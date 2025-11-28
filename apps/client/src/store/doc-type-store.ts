import type { Document } from '@/types/document-types';
import type { StateCreator } from 'zustand';

export type DocumentTypeStoreSliceType = {
    documentTypeEditTarget: Document | null;
    isDocumentTypeEditDialogOpen: boolean;
    documentTypeDeleteTargetId: string | null;
    isDocumentTypeDeleteDialogOpen: boolean;

    openDocumentTypeEditDialog: (doc: Document | null) => void;
    closeDocumentTypeEditDialog: () => void;
    setDocumentTypeEditDialogOpen: (state: boolean) => void;
    updateDocumentTypeEditTarget: (updates: Partial<Document>) => void;

    openDocumentTypeDeleteDialog: (id: string) => void;
    closeDocumentTypeDeleteDialog: () => void;
    setDocumentTypeDeleteDialogOpen: (state: boolean) => void;
};

export const createDocTypeSlice: StateCreator<DocumentTypeStoreSliceType> = (
    set
) => ({
    documentTypeEditTarget: null,
    isDocumentTypeEditDialogOpen: false,
    documentTypeDeleteTargetId: null,
    isDocumentTypeDeleteDialogOpen: false,

    openDocumentTypeEditDialog: (doc) => {
        set({
            documentTypeEditTarget: doc,
            isDocumentTypeEditDialogOpen: true,
        });
    },

    closeDocumentTypeEditDialog: () =>
        set({
            documentTypeEditTarget: null,
            isDocumentTypeEditDialogOpen: false,
        }),

    setDocumentTypeEditDialogOpen: (state) =>
        set(() => ({ isDocumentTypeEditDialogOpen: state })),

    updateDocumentTypeEditTarget: (updates: Partial<Document>) =>
        set((state) => ({
            documentTypeEditTarget: state.documentTypeEditTarget
                ? { ...state.documentTypeEditTarget, ...updates }
                : null,
        })),

    openDocumentTypeDeleteDialog: (id) => {
        set({
            documentTypeDeleteTargetId: id,
            isDocumentTypeDeleteDialogOpen: true,
        });
    },

    closeDocumentTypeDeleteDialog: () =>
        set({
            documentTypeDeleteTargetId: null,
            isDocumentTypeDeleteDialogOpen: false,
        }),

    setDocumentTypeDeleteDialogOpen: (state) =>
        set(() => ({ isDocumentTypeDeleteDialogOpen: state })),
});
