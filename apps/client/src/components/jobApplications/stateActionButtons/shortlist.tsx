import { useMoveJobApplicationStatus } from '@/api/job-application-api';
import { Button } from '@/components/ui/button';
import { useAccessChecker } from '@/hooks/use-has-access';
import { JOB_APPLICATION_STATUS } from '@/types/enums';
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
    moveTo: z.enum([...Object.values(JOB_APPLICATION_STATUS)]),
}) satisfies z.ZodType<MoveJobApplicationStatusCommandCorrected>;

export const ShortlistButton = ({ jobApplicationId, visibleTo }: Props) => {
    const moveJobApplicationStatusMutation = useMoveJobApplicationStatus();
    const canAccess = useAccessChecker();

    const form = useForm<z.infer<typeof moveJobApplicationStatusFormSchema>>({
        resolver: zodResolver(moveJobApplicationStatusFormSchema),
        defaultValues: {
            id: jobApplicationId,
            moveTo: JOB_APPLICATION_STATUS.SHORTLISTED,
        },
    });

    const onSubmit = async (data: MoveJobApplicationStatusCommandCorrected) => {
        moveJobApplicationStatusMutation.mutate(data);
    };

    if (!canAccess(visibleTo)) return null;

    return (
        <form onSubmit={form.handleSubmit(onSubmit)}>
            <Button
                variant="outline"
                type="submit"
                disabled={moveJobApplicationStatusMutation.isPending}
            >
                Shortlist
            </Button>
        </form>
    );
};
