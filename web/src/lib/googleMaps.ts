/// <reference types="google.maps" />

import { importLibrary, setOptions } from '@googlemaps/js-api-loader';

let mapsConfigured = false;
let placesLibraryPromise: Promise<google.maps.PlacesLibrary> | null = null;

export const loadPlacesLibrary = async () => {
  const apiKey = import.meta.env.VITE_GOOGLE_MAPS_API_KEY;

  if (!apiKey) {
    throw new Error('Google Maps API key is missing.');
  }

  if (!mapsConfigured) {
    setOptions({
      key: apiKey,
      libraries: ['places'],
    });
    mapsConfigured = true;
  }

  if (!placesLibraryPromise) {
    placesLibraryPromise = importLibrary('places') as Promise<google.maps.PlacesLibrary>;
  }

  return placesLibraryPromise;
};
