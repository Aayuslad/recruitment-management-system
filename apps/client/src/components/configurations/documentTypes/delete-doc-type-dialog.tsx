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
        docTypeDeleteTargetId,
        docTypeDeleteDialog,
        closeDocTypeDeleteDialog,
        setDocTypeDeleteDialog,
    } = useAppStore(
        useShallow((s) => ({
            docTypeDeleteTargetId: s.docTypeDeleteTargetId,
            docTypeDeleteDialog: s.docTypeDeleteDialog,
            closeDocTypeDeleteDialog: s.closeDocTypeDeleteDialog,
            setDocTypeDeleteDialog: s.setDocTypeDeleteDialog,
        }))
    );
    const deleteDocumentTypeMutation = useDeleteDocumentType();

    const submit = async (id: string | null) => {
        if (!id) {
            return;
        }
        deleteDocumentTypeMutation.mutate(id, {
            onSuccess: () => {
                closeDocTypeDeleteDialog();
            },
        });
    };

    return (
        <Dialog
            open={docTypeDeleteDialog}
            onOpenChange={setDocTypeDeleteDialog}
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
                        onClick={() => submit(docTypeDeleteTargetId)}
                        disabled={deleteDocumentTypeMutation.isPending}
                    >
                        Delete
                    </Button>
                </DialogFooter>
            </DialogContent>
        </Dialog>
    );
}
