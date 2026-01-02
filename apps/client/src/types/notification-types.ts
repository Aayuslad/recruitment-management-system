import type { components } from './generated/api';

export interface Notification {
    id: string;
    fromUserId?: string | null;
    fromUserName?: string | null;
    subject: string;
    message: string;
    isRead: boolean;
}

export type MarkNotificationsAsReadCommandCorrected = Omit<
    components['schemas']['MarkNotificationsAsReadCommand'],
    'notifications'
> & {
    notifications: (Omit<components['schemas']['NotificationDTO'], 'id'> & {
        id: string;
    })[];
};
