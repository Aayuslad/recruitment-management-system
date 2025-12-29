import { useMoveJobApplicationStatus } from '@/api/job-application-api';
import {
    AlertDialog,
    AlertDialogCancel,
    AlertDialogContent,
    AlertDialogDescription,
    AlertDialogFooter,
    AlertDialogHeader,
    AlertDialogTitle,
    AlertDialogTrigger,
} from '@/components/ui/alert-dialog';
import { Button } from '@/components/ui/button';
import { useAccessChecker } from '@/hooks/use-has-access';
import { JOB_APPLICATION_ACTION_TYPE } from '@/types/enums';
import type { MoveJobApplicationStatusCommandCorrected } from '@/types/job-application-types';
import { zodResolver } from '@hookform/resolvers/zod';
import { useForm } from 'react-hook-form';
import z from 'zod';

type Props = {
    jobApplicationId: string;
    visibleTo: string[];
};

const moveJobApplicationStatusFormSchema = z.object({
    id: z.string(),
    action: z.enum(['UnHold', 'RollBack']),
}) satisfies z.ZodType<MoveJobApplicationStatusCommandCorrected>;

export const UnHoldButton = ({ jobApplicationId, visibleTo }: Props) => {
    const moveJobApplicationStatusMutation = useMoveJobApplicationStatus();
    const canAccess = useAccessChecker();

    const form = useForm<z.infer<typeof moveJobApplicationStatusFormSchema>>({
        resolver: zodResolver(moveJobApplicationStatusFormSchema),
        defaultValues: {
            id: jobApplicationId,
            action: JOB_APPLICATION_ACTION_TYPE.UNHOLD,
        },
    });

    const onSubmit = async (data: MoveJobApplicationStatusCommandCorrected) => {
        moveJobApplicationStatusMutation.mutate(data);
    };

    if (!canAccess(visibleTo)) return null;

    return (
        <AlertDialog>
            <AlertDialogTrigger asChild>
                <Button variant="outline" type="button">
                    Resume
                </Button>
            </AlertDialogTrigger>
            <AlertDialogContent>
                <form onSubmit={form.handleSubmit(onSubmit)}>
                    <AlertDialogHeader>
                        <AlertDialogTitle>
                            Are you absolutely sure?
                        </AlertDialogTitle>
                        <AlertDialogDescription>
                            This action will rollback the job application to its
                            previous status before hold.
                        </AlertDialogDescription>
                    </AlertDialogHeader>
                    <AlertDialogFooter>
                        <AlertDialogCancel>Cancel</AlertDialogCancel>
                        <Button
                            type="submit"
                            variant="destructive"
                            disabled={
                                moveJobApplicationStatusMutation.isPending
                            }
                        >
                            Continue
                        </Button>
                    </AlertDialogFooter>
                </form>
            </AlertDialogContent>
        </AlertDialog>
    );
};
