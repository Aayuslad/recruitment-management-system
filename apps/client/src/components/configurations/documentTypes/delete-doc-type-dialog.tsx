import { useDeleteDocumentType } from '@/api/document-api';
import { Button } from '@/components/ui/button';
import {
    Dialog,
    DialogClose,
    DialogContent,
    DialogDescription,
    DialogFooter,
    DialogHeader,
    DialogTitle,
} from '@/components/ui/dialog';
import { useAppStore } from '@/store';
import { useShallow } from 'zustand/react/shallow';

export function DeleteDocTypeDialog() {
    const {
        documentTypeDeleteTargetId,
        isDocumentTypeDeleteDialogOpen,
        closeDocumentTypeDeleteDialog,
        setDocumentTypeDeleteDialogOpen,
    } = useAppStore(
        useShallow((s) => ({
            documentTypeDeleteTargetId: s.documentTypeDeleteTargetId,
            isDocumentTypeDeleteDialogOpen: s.isDocumentTypeDeleteDialogOpen,
            closeDocumentTypeDeleteDialog: s.closeDocumentTypeDeleteDialog,
            setDocumentTypeDeleteDialogOpen: s.setDocumentTypeDeleteDialogOpen,
        }))
    );
    const deleteDocumentTypeMutation = useDeleteDocumentType();

    const submit = async (id: string) => {
        deleteDocumentTypeMutation.mutate(id, {
            onSuccess: () => {
                closeDocumentTypeDeleteDialog();
            },
        });
    };

    return (
        <Dialog
            open={isDocumentTypeDeleteDialogOpen}
            onOpenChange={setDocumentTypeDeleteDialogOpen}
        >
            <DialogContent className="sm:max-w-[425px] space-y-2">
                <DialogHeader className="space-y-2">
                    <DialogTitle>Confirm Delete Document Type</DialogTitle>
                    <DialogDescription>
                        This document type will be removed and this action
                        cannot be undone.
                    </DialogDescription>
                </DialogHeader>

                <DialogFooter>
                    <DialogClose asChild className="flex-1">
                        <Button
                            variant="secondary"
                            disabled={deleteDocumentTypeMutation.isPending}
                        >
                            Cancel
                        </Button>
                    </DialogClose>
                    <Button
                        type="submit"
                        variant="default"
                        className="flex-1 bg-red-500 text-white hover:bg-red-600"
                        onClick={() => submit(documentTypeDeleteTargetId!)}
                        disabled={deleteDocumentTypeMutation.isPending}
                    >
                        Delete
                    </Button>
                </DialogFooter>
            </DialogContent>
        </Dialog>
    );
}
