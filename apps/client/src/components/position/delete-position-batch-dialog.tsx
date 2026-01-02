import { useDeletePositionBatch } from '@/api/position-api';
import { Button } from '@/components/ui/button';
import {
    Dialog,
    DialogContent,
    DialogDescription,
    DialogFooter,
    DialogHeader,
    DialogTitle,
} from '@/components/ui/dialog';
import { useAccessChecker } from '@/hooks/use-has-access';
import { DialogClose } from '@radix-ui/react-dialog';
import { useState } from 'react';
import { useNavigate } from 'react-router-dom';

type Props = {
    visibleTo: string[];
    positionBatchId: string;
};

export function DeletePositionBatchDialog({
    positionBatchId,
    visibleTo,
}: Props) {
    const [open, setOpen] = useState(false);
    const deletePositionBatchMutation = useDeletePositionBatch();
    const canAccess = useAccessChecker();
    const navigate = useNavigate();

    if (!canAccess(visibleTo)) return null;

    return (
        <>
            <Button
                variant="secondary"
                className="hover:bg-red-600 w-[160px]"
                disabled={deletePositionBatchMutation.isPending}
                onClick={() => setOpen(true)}
            >
                Delete Batch
            </Button>

            <Dialog open={open} onOpenChange={setOpen}>
                <DialogContent className="sm:max-w-[425px] space-y-2">
                    <DialogHeader className="space-y-2">
                        <DialogTitle>Delete this position batch?</DialogTitle>
                        <DialogDescription>
                            All positions inside this batch will also be
                            removed.
                            <br />
                            This action cannot be undone.
                        </DialogDescription>
                    </DialogHeader>

                    <DialogFooter>
                        <DialogClose asChild className="flex-1">
                            <Button
                                variant="secondary"
                                disabled={deletePositionBatchMutation.isPending}
                            >
                                Cancel
                            </Button>
                        </DialogClose>
                        <Button
                            type="submit"
                            variant="default"
                            className="flex-1 bg-red-500 text-white hover:bg-red-600"
                            onClick={() => {
                                deletePositionBatchMutation.mutate(
                                    positionBatchId,
                                    {
                                        onSuccess: () => {
                                            setOpen(false);
                                            navigate('/positions');
                                        },
                                    }
                                );
                            }}
                            disabled={deletePositionBatchMutation.isPending}
                        >
                            Delete
                        </Button>
                    </DialogFooter>
                </DialogContent>
            </Dialog>
        </>
    );
}
