import apiClient from '@/lib/client';
import type { ClientRequest } from '@/features/clients/types';

export type ClientListItem = {
  id: string;
  type: number;
  displayName: string;
  email?: string | null;
  phoneNumber?: string | null;
  address?: string | null;
};

export type ClientQueryFilter = {
  searchTerm?: string;
  clientType?: number | null;
  hasActiveJobs?: boolean | null;
  pageNumber?: number;
  pageSize?: number;
  sortColumn?: string;
  sortDescending?: boolean;
};

export type PageResult<T> = {
  items: T[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
};

const normalizeQueryFilter = (filter: ClientQueryFilter = {}) => {
  const params: Record<string, string | number | boolean> = {};

  if (filter.searchTerm && filter.searchTerm.trim()) {
    params.searchTerm = filter.searchTerm.trim();
  }

  if (filter.clientType !== undefined && filter.clientType !== null) {
    params.clientType = filter.clientType;
  }

  if (filter.hasActiveJobs !== undefined && filter.hasActiveJobs !== null) {
    params.hasActiveJobs = filter.hasActiveJobs;
  }

  if (filter.pageNumber !== undefined) {
    params.pageNumber = filter.pageNumber;
  }

  if (filter.pageSize !== undefined) {
    params.pageSize = filter.pageSize;
  }

  if (filter.sortColumn) {
    params.sortColumn = filter.sortColumn;
  }

  if (filter.sortDescending !== undefined) {
    params.sortDescending = filter.sortDescending;
  }

  return params;
};

export const clientService = {
  getAll: (filter: ClientQueryFilter = {}) =>
    apiClient.get<PageResult<ClientListItem>>('/clients', {
      params: normalizeQueryFilter(filter),
    }),
  getById: (id: string) => apiClient.get<ClientRequest>(`/clients/${id}`),
  create: (data: ClientRequest) => apiClient.post('/clients', data),
  update: (id: string, data: ClientRequest) => apiClient.put(`/clients/${id}`, data),
};
