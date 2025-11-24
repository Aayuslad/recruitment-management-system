import type {
    Document,
    CreateDocumentTypeCommandCorrected,
} from '@/types/document-types';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import axios, { AxiosError } from 'axios';
import { toast } from 'sonner';

export function useCreateDocumentType() {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: async (
            payload: CreateDocumentTypeCommandCorrected
        ): Promise<void> => {
            await axios.post('/document', payload);
        },
        onSuccess: () => {
            toast.success('Document Created');
            queryClient.invalidateQueries({ queryKey: ['documents'] });
        },
        onError: (error: AxiosError<{ error: string }>) => {
            toast.error(
                error.response?.data?.error ||
                    error.message ||
                    'Document Not Created'
            );
            console.error('Document creation failed:', error);
        },
    });
}

export function useDeleteDocumentType() {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: async (id: string): Promise<void> => {
            await axios.delete(`/document/${id}`);
        },
        onSuccess: () => {
            toast.success('Document Deleted');
            queryClient.invalidateQueries({ queryKey: ['documents'] });
        },
        onError: (error: AxiosError<{ error: string }>) => {
            toast.error(
                error.response?.data?.error ||
                    error.message ||
                    'Document Not Deleted'
            );
            console.error('Document deletion failed:', error);
        },
    });
}

export function useGetDocumentTypes() {
    return useQuery({
        queryKey: ['documents'],
        queryFn: async (): Promise<Document[]> => {
            const { data } = await axios.get('/document');
            return data;
        },
    });
}
