import type { Address } from '@/shared/types';

/** The fields that must be requested via `Place.fetchFields` for `buildAddressFromPlace` to work correctly */
export const PLACE_FIELDS = [
  'id',
  'formattedAddress',
  'displayName',
  'addressComponents',
  'location',
];

/**
 * Finds the first address component of a given type (e.g. "postal_code") and returns its long text. Returns an empty string if the component is missing.
 * @param components The array of address components from a Google Place object
 * @param type The type of address component to find (e.g., "postal_code")
 * @returns The long text of the first matching address component, or an empty string if not found
 */
const getComponentValue = (
    components: google.maps.places.AddressComponent[] | undefined,
    type: string
) => components?.find((component) => component.types.includes(type))?.longText ?? '';

/**
 * Converts a Google Place object into an Address type.
 * 
 * The place must have had its fields fetched first (see `PLACE_FIELDS`). otherwise some address components may be undefined.
 * @param place The Google Place object to build the address from
 * @returns The constructed Address object, or null if the place has no ID
 */
export const buildAddressFromPlace = (place: google.maps.places.Place): Address | null => {
    if (!place.id) {
        return null;
    }

    const components = place.addressComponents;

    return {
        placeId: place.id,
        displayName: place.formattedAddress ?? place.displayName ?? '',
        streetNumber: getComponentValue(components, 'street_number'),
        streetName: getComponentValue(components, 'route'),
        suburb: 
            getComponentValue(components, 'locality') ||
            getComponentValue(components, 'sublocality') ||
            getComponentValue(components, 'postal_town') ||
            getComponentValue(components, 'administrative_area_level_2'),
        postCode: getComponentValue(components, 'postal_code'),
        state: getComponentValue(components, 'administrative_area_level_1'),
        country: getComponentValue(components, 'country'),
        latitude: place.location?.lat(),
        longitude: place.location?.lng(),
    };
};
