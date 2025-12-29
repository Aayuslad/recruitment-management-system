import type { Designation } from '@/types/designation-types';
import type { StateCreator } from 'zustand';

export type DesignationStoreSliceType = {
    designationDialogTarget: Designation | null;
    designationDialog: boolean;
    designationDialogTab: 'view' | 'edit';
    designationDeleteTargetId: string | null;
    designationDeleteDialog: boolean;

    openDesignationDialog: (
        tab: 'view' | 'edit',
        designation: Designation | null
    ) => void;
    closeDesignationDialog: () => void;
    setDesignationDialog: (state: boolean) => void;
    setDesignationDialogTab: (tab: 'view' | 'edit') => void;
    updateDesignationDialogTarget: (updates: Partial<Designation>) => void;

    openDesignationDeleteDialog: (id: string) => void;
    closeDesignationDeleteDialog: () => void;
    setDesignationDeleteDialog: (state: boolean) => void;
};

export const createDesignationSlice: StateCreator<DesignationStoreSliceType> = (
    set
) => ({
    designationDialogTarget: null,
    designationDialog: false,
    designationDialogTab: 'view',
    designationDeleteTargetId: null,
    designationDeleteDialog: false,

    openDesignationDialog: (tab, designation) => {
        set({
            designationDialogTab: tab,
            designationDialogTarget: designation,
            designationDialog: true,
        });
    },

    closeDesignationDialog: () =>
        set({ designationDialogTarget: null, designationDialog: false }),

    setDesignationDialog: (state) => set(() => ({ designationDialog: state })),

    setDesignationDialogTab: (tab) =>
        set(() => ({ designationDialogTab: tab })),

    updateDesignationDialogTarget: (updates: Partial<Designation>) =>
        set((state) => ({
            designationDialogTarget: state.designationDialogTarget
                ? { ...state.designationDialogTarget, ...updates }
                : null,
        })),

    openDesignationDeleteDialog: (id) => {
        set({ designationDeleteTargetId: id, designationDeleteDialog: true });
    },

    closeDesignationDeleteDialog: () =>
        set({
            designationDeleteTargetId: null,
            designationDeleteDialog: false,
        }),

    setDesignationDeleteDialog: (state) =>
        set(() => ({ designationDeleteDialog: state })),
});
