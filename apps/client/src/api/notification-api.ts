import type {
    MarkNotificationsAsReadCommandCorrected,
    Notification,
} from '@/types/notification-types';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import axios, { AxiosError } from 'axios';
import { toast } from 'sonner';

export function useMarkNotificationsAsRead() {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: async (
            payload: MarkNotificationsAsReadCommandCorrected
        ): Promise<void> => {
            await axios.post('/notification/mark-as-read', payload);
        },
        onSuccess: () => {
            toast.success('Notifications Marked as Read');
            queryClient.invalidateQueries({ queryKey: ['notifications'] });
        },
        onError: (error: AxiosError<{ error: string }>) => {
            toast.error(
                error.response?.data?.error ||
                    error.message ||
                    'Failed to Mark Notifications as Read'
            );
            console.error('Marking notifications as read failed:', error);
        },
    });
}

export function useGetUserNotifications(userId: string) {
    return useQuery({
        queryKey: ['notifications', userId],
        queryFn: async (): Promise<Notification[]> => {
            const { data } = await axios.get(`/notification/user/${userId}`);
            return data;
        },
    });
}
