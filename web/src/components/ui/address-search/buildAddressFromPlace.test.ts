/// <reference types="google.maps" />

import { describe, expect, it } from 'vitest';
import { buildAddressFromPlace } from './buildAddressFromPlace';

const component = (type: string, longText:string) => ({ types: [type], longText });

const makePlace = (overrides: Record<string, unknown> ={}) => ({
    id: 'place-123',
    formatted_address: '12 Hay St, Perth WA 6000, Australia',
    displayName: '12 Hay St',
    addressComponents: [
        component('street_number', '12'),
        component('route', 'Hay Street'),
        component('locality', 'Perth'),
        component('administrative_area_level_1', 'Western Australia'),
        component('postal_code', '6000'),
        component('country', 'Australia')
    ],
    location: {
        lat: () => -31.95,
        lng: () => 115.86
    },
    ...overrides,
}) as unknown as google.maps.places.Place;

describe('buildAddressFromPlace', () => {
    it('maps a complete place to an Address', () => {
        expect(buildAddressFromPlace(makePlace())).toEqual({
            placeId: 'place-123',
            displayName: '12 Hay St',
            streetNumber: '12',
            streetName: 'Hay Street',
            suburb: 'Perth',
            state: 'Western Australia',
            postCode: '6000',
            country: 'Australia',
            latitude: -31.95,
            longitude: 115.86
        });
    });

    it('returns null when the place has no id', () => {
        expect(buildAddressFromPlace(makePlace({ id: undefined }))).toBeNull();
    });

    it('falls back to displayName when there is no formattedAddress', () => {
        const address = buildAddressFromPlace(makePlace({ formatted_address: undefined }));
        expect(address?.displayName).toBe('12 Hay St');
    });

    it('uses empty string for missing components', () => {
        const address = buildAddressFromPlace(makePlace({ addressComponents: [] }));
        expect(address).toMatchObject({
            streetNumber: '',
            streetName: '',
            suburb: '',
            state: '',
            postCode: '',
            country: ''
        });
    });

    it('leaves coordinates undefined when there is no location', () => {
        const address = buildAddressFromPlace(makePlace({ location: undefined }));
        expect(address?.latitude).toBeUndefined();
        expect(address?.longitude).toBeUndefined();
    });

    describe('suburb fallbacks', () => {
        it('prefers locality over everrything else', () => {
            const address = buildAddressFromPlace(makePlace({
                addressComponents: [
                    component('locality', 'Perth'),
                    component('sublocality', 'Northbridge'),
                    component('postal_town', 'Town')
                ]
            }));

            expect(address?.suburb).toBe('Perth');
        });

        it('falls back to sublocality, then postal_town, then administrative_area_level_2', () => {
            const withTypes = (...types: [string, string][]) =>
                buildAddressFromPlace(makePlace({
                    addressComponents: types.map(([type, text]) => component(type, text))
                }))?.suburb;
            expect(withTypes(['sublocality', 'Northbridge'], ['postal_town', 'Town'])).toBe('Northbridge');
            expect(withTypes(['postal_town', 'Town'], ['administrative_area_level_2', 'Shire'])).toBe('Town');
            expect(withTypes(['administrative_area_level_2', 'Shire'])).toBe('Shire');
        });
    });
});