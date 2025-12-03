import { useDeleteDesignation } from '@/api/designation-api';
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

export function DeleteDesignationDialog() {
    const {
        designationDeleteTargetId,
        designationDeleteDialog,
        closeDesignationDeleteDialog,
        setDesignationDeleteDialog,
    } = useAppStore(
        useShallow((s) => ({
            designationDeleteTargetId: s.designationDeleteTargetId,
            designationDeleteDialog: s.designationDeleteDialog,
            closeDesignationDeleteDialog: s.closeDesignationDeleteDialog,
            setDesignationDeleteDialog: s.setDesignationDeleteDialog,
        }))
    );
    const deleteDesignationMutation = useDeleteDesignation();

    const submit = async (id: string) => {
        deleteDesignationMutation.mutate(id, {
            onSuccess: () => {
                closeDesignationDeleteDialog();
            },
        });
    };

    return (
        <Dialog
            open={designationDeleteDialog}
            onOpenChange={setDesignationDeleteDialog}
        >
            <DialogContent className="sm:max-w-[425px] space-y-2">
                <DialogHeader className="space-y-2">
                    <DialogTitle>Confirm Delete Designation</DialogTitle>
                    <DialogDescription>
                        This Designation will be removed and this action cannot
                        be undone.
                    </DialogDescription>
                </DialogHeader>

                <DialogFooter>
                    <DialogClose asChild className="flex-1">
                        <Button
                            variant="secondary"
                            disabled={deleteDesignationMutation.isPending}
                        >
                            Cancel
                        </Button>
                    </DialogClose>
                    <Button
                        type="submit"
                        variant="default"
                        className="flex-1 bg-red-500 text-white hover:bg-red-600"
                        onClick={() => submit(designationDeleteTargetId!)}
                        disabled={deleteDesignationMutation.isPending}
                    >
                        Delete
                    </Button>
                </DialogFooter>
            </DialogContent>
        </Dialog>
    );
}
