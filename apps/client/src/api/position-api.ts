import type {
    Position,
    PositionSummary,
    PositionBatch,
    PositionBatchSummary,
    CreatePositionBatchCommandCorrected,
    EditPositionBatchCommandCorrected,
    ClosePositionCommandCorrected,
    SetPositionOnHoldCommandCorrected,
    BatchPositionsSummary,
} from '@/types/position-types';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import axios, { AxiosError } from 'axios';
import { toast } from 'sonner';

export function useCreatePositionBatch() {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: async (
            payload: CreatePositionBatchCommandCorrected
        ): Promise<void> => {
            await axios.post('/position/batch', payload);
        },
        onSuccess: () => {
            toast.success('Position batch created');
            queryClient.invalidateQueries({ queryKey: ['position-batches'] });
        },
        onError: (error: AxiosError<{ error: string }>) => {
            toast.error(
                error.response?.data?.error ||
                    error.message ||
                    'Failed to create position batch'
            );
            console.error(error);
        },
    });
}

export function useEditPositionBatch() {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: async (
            payload: EditPositionBatchCommandCorrected
        ): Promise<void> => {
            await axios.put(
                `/position/batch/${payload.positionBatchId}`,
                payload
            );
        },
        onSuccess: (_, variables) => {
            toast.success('Position batch updated');
            queryClient.invalidateQueries({ queryKey: ['position-batches'] });
            queryClient.invalidateQueries({
                queryKey: ['position-batch', variables.positionBatchId],
            });
        },
        onError: (error: AxiosError<{ error: string }>) => {
            toast.error(
                error.response?.data?.error ||
                    error.message ||
                    'Failed to update position batch'
            );
            console.error(error);
        },
    });
}

export function useDeletePositionBatch() {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: async (id: string): Promise<void> => {
            await axios.delete(`/position/batch/${id}`);
        },
        onSuccess: () => {
            toast.success('Position batch deleted');
            queryClient.invalidateQueries({ queryKey: ['position-batches'] });
        },
        onError: (error: AxiosError<{ error: string }>) => {
            toast.error(
                error.response?.data?.error ||
                    error.message ||
                    'Failed to delete position batch'
            );
            console.error(error);
        },
    });
}

export function useClosePosition() {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: async (
            payload: ClosePositionCommandCorrected
        ): Promise<void> => {
            await axios.put(`/position/close/${payload.positionId}`, payload);
        },
        onSuccess: (_, variables) => {
            toast.success('Position closed');
            queryClient.invalidateQueries({ queryKey: ['positions'] });
            queryClient.invalidateQueries({
                queryKey: ['position', variables.positionId],
            });
        },
        onError: (error: AxiosError<{ error: string }>) => {
            toast.error(
                error.response?.data?.error ||
                    error.message ||
                    'Failed to close position'
            );
            console.error(error);
        },
    });
}

export function useSetPositionOnHold() {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: async (
            payload: SetPositionOnHoldCommandCorrected
        ): Promise<void> => {
            await axios.put(`/position/onHold/${payload.positionId}`, payload);
        },
        onSuccess: (_, variables) => {
            toast.success('Position status updated to on hold');
            queryClient.invalidateQueries({ queryKey: ['positions'] });
            queryClient.invalidateQueries({
                queryKey: ['position', variables.positionId],
            });
        },
        onError: (error: AxiosError<{ error: string }>) => {
            toast.error(
                error.response?.data?.error ||
                    error.message ||
                    'Failed to set position on hold'
            );
            console.error(error);
        },
    });
}

export function useGetPositionBatch(id?: string) {
    return useQuery({
        queryKey: ['position-batch', id],
        queryFn: async (): Promise<PositionBatch | null> => {
            const { data } = await axios.get(`/position/batch/${id}`);
            return data;
        },
        enabled: !!id,
    });
}

export function useGetPositionBatches() {
    return useQuery({
        queryKey: ['position-batches'],
        queryFn: async (): Promise<PositionBatchSummary[]> => {
            const { data } = await axios.get('/position/batch');
            return data;
        },
    });
}

export function useGetPosition(id: string) {
    return useQuery({
        queryKey: ['position', id],
        queryFn: async (): Promise<Position | null> => {
            const { data } = await axios.get(`/position/${id}`);
            return data;
        },
    });
}

export function useGetPositions() {
    return useQuery({
        queryKey: ['positions'],
        queryFn: async (): Promise<PositionSummary[]> => {
            const { data } = await axios.get('/position');
            return data;
        },
    });
}

export function useGetBatchPositions(batchId: string) {
    return useQuery({
        queryKey: ['batch-positions', batchId],
        queryFn: async (): Promise<BatchPositionsSummary[]> => {
            const { data } = await axios.get(
                `/position/batch-positions/${batchId}`
            );
            return data;
        },
    });
}
