import type { UsersDetail } from '@/types/user-types';
import type { StateCreator } from 'zustand';

export type AdminStoreSliceType = {
    userEditTarget: UsersDetail | null;
    userEditDialog: boolean;

    openUserEditDialog: (user: UsersDetail | null) => void;
    closeUserEditDialog: () => void;
    setUserEditDialog: (state: boolean) => void;
    updateUserEditTarget: (updates: Partial<UsersDetail>) => void;
};

export const createAdminSlice: StateCreator<AdminStoreSliceType> = (set) => ({
    userEditTarget: null,
    userEditDialog: false,

    openUserEditDialog: (user) => {
        set({ userEditTarget: user, userEditDialog: true });
    },

    closeUserEditDialog: () =>
        set({ userEditTarget: null, userEditDialog: false }),

    setUserEditDialog: (state) => set(() => ({ userEditDialog: state })),

    updateUserEditTarget: (updates: Partial<UsersDetail>) =>
        set((state) => ({
            userEditTarget: state.userEditTarget
                ? { ...state.userEditTarget, ...updates }
                : null,
        })),
});
