import { useDeleteCandidate } from '@/api/candidate-api';
import { Button } from '@/components/ui/button';
import {
    Dialog,
    DialogContent,
    DialogDescription,
    DialogFooter,
    DialogHeader,
    DialogTitle,
} from '@/components/ui/dialog';
import { DialogClose } from '@radix-ui/react-dialog';
import { useState } from 'react';
import { useNavigate } from 'react-router-dom';

interface Props {
    id: string;
}

export function DeleteCandidateDialog({ id }: Props) {
    const [open, setOpen] = useState(false);
    const deleteCandidateMutation = useDeleteCandidate();
    const navigate = useNavigate();

    return (
        <>
            <Button
                variant="secondary"
                className="hover:bg-red-600 w-[160px]"
                disabled={deleteCandidateMutation.isPending}
                onClick={() => setOpen(true)}
            >
                Delete Candidate
            </Button>

            <Dialog open={open} onOpenChange={setOpen}>
                <DialogContent className="sm:max-w-[425px] space-y-2">
                    <DialogHeader className="space-y-2">
                        <DialogTitle>Delete this candidate?</DialogTitle>
                        <DialogDescription>
                            All applications by this cadidate will also be
                            removed.
                            <br />
                            This action cannot be undone.
                        </DialogDescription>
                    </DialogHeader>

                    <DialogFooter>
                        <DialogClose asChild className="flex-1">
                            <Button
                                variant="secondary"
                                disabled={deleteCandidateMutation.isPending}
                            >
                                Cancel
                            </Button>
                        </DialogClose>
                        <Button
                            type="submit"
                            variant="default"
                            className="flex-1 bg-red-500 text-white hover:bg-red-600"
                            onClick={() => {
                                deleteCandidateMutation.mutate(id, {
                                    onSuccess: () => {
                                        setOpen(false);
                                        navigate('/candidates');
                                    },
                                });
                            }}
                            disabled={deleteCandidateMutation.isPending}
                        >
                            Delete
                        </Button>
                    </DialogFooter>
                </DialogContent>
            </Dialog>
        </>
    );
}
