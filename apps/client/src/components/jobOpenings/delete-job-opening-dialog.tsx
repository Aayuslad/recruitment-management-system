import { useDeleteJobApplication } from '@/api/job-application-api';
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

interface Props {
    jobOpeningId: string;
    visibleTo: string[];
}

export function DeleteJobOpeningDialog({ jobOpeningId, visibleTo }: Props) {
    const [open, setOpen] = useState(false);
    const canAccess = useAccessChecker();
    const deleteJobApplicationMutation = useDeleteJobApplication();
    const navigate = useNavigate();

    if (!canAccess(visibleTo)) return null;

    return (
        <>
            <Button
                variant="secondary"
                className="hover:bg-red-600 w-[160px]"
                disabled={deleteJobApplicationMutation.isPending}
                onClick={() => setOpen(true)}
            >
                Delete Job Opening
            </Button>

            <Dialog open={open} onOpenChange={setOpen}>
                <DialogContent className="sm:max-w-[425px] space-y-2">
                    <DialogHeader className="space-y-2">
                        <DialogTitle>Delete this job opening?</DialogTitle>
                        <DialogDescription>
                            All applications inside this opening will also be
                            removed.
                            <br />
                            This action cannot be undone.
                        </DialogDescription>
                    </DialogHeader>

                    <DialogFooter>
                        <DialogClose asChild className="flex-1">
                            <Button
                                variant="secondary"
                                disabled={
                                    deleteJobApplicationMutation.isPending
                                }
                            >
                                Cancel
                            </Button>
                        </DialogClose>
                        <Button
                            type="submit"
                            variant="default"
                            className="flex-1 bg-red-500 text-white hover:bg-red-600"
                            onClick={() => {
                                deleteJobApplicationMutation.mutate(
                                    jobOpeningId,
                                    {
                                        onSuccess: () => {
                                            setOpen(false);
                                            navigate('/job-openings');
                                        },
                                    }
                                );
                            }}
                            disabled={deleteJobApplicationMutation.isPending}
                        >
                            Delete
                        </Button>
                    </DialogFooter>
                </DialogContent>
            </Dialog>
        </>
    );
}
