import type { Document } from '@/types/document-types';
import type { StateCreator } from 'zustand';

export type DocumentTypeStoreSliceType = {
    docTypeEditTarget: Document | null;
    docTypeEditDialog: boolean;
    docTypeDeleteTargetId: string | null;
    docTypeDeleteDialog: boolean;

    openDocTypeEditDialog: (doc: Document | null) => void;
    closeDocTypeEditDialog: () => void;
    setDocTypeEditDialog: (state: boolean) => void;
    updateDocTypeEditTarget: (updates: Partial<Document>) => void;

    openDocTypeDeleteDialog: (id: string) => void;
    closeDocTypeDeleteDialog: () => void;
    setDocTypeDeleteDialog: (state: boolean) => void;
};

export const createDocTypeSlice: StateCreator<DocumentTypeStoreSliceType> = (
    set
) => ({
    docTypeEditTarget: null,
    docTypeEditDialog: false,
    docTypeDeleteTargetId: null,
    docTypeDeleteDialog: false,

    openDocTypeEditDialog: (doc) => {
        set({
            docTypeEditTarget: doc,
            docTypeEditDialog: true,
        });
    },

    closeDocTypeEditDialog: () =>
        set({
            docTypeEditTarget: null,
            docTypeEditDialog: false,
        }),

    setDocTypeEditDialog: (state) => set(() => ({ docTypeEditDialog: state })),

    updateDocTypeEditTarget: (updates: Partial<Document>) =>
        set((state) => ({
            docTypeEditTarget: state.docTypeEditTarget
                ? { ...state.docTypeEditTarget, ...updates }
                : null,
        })),

    openDocTypeDeleteDialog: (id) => {
        set({
            docTypeDeleteTargetId: id,
            docTypeDeleteDialog: true,
        });
    },

    closeDocTypeDeleteDialog: () =>
        set({
            docTypeDeleteTargetId: null,
            docTypeDeleteDialog: false,
        }),

    setDocTypeDeleteDialog: (state) =>
        set(() => ({ docTypeDeleteDialog: state })),
});
