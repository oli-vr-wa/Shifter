import { useQueryClient, useMutation } from "@tanstack/react-query";
import type { ClientRequest } from "../types";
import { clientService } from "../api/client.service";

export const useSaveClient = (isEditMode: boolean, onSuccess: () => void) => {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: (data: ClientRequest) =>
            isEditMode && data.id ? clientService.update(data.id, data) : clientService.create(data),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["clients"] });
            onSuccess();
        }
    });
};