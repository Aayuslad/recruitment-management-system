import { useMoveJobApplicationStatus } from '@/api/job-application-api';
import { Button } from '@/components/ui/button';
import { JOB_APPLICATION_STATUS } from '@/types/enums';
import type { MoveJobApplicationStatusCommandCorrected } from '@/types/job-application-types';
import { zodResolver } from '@hookform/resolvers/zod';
import { useForm } from 'react-hook-form';
import z from 'zod';

type Props = {
    jobApplicationId: string;
};

const moveJobApplicationStatusFormSchema = z.object({
    id: z.string(),
    moveTo: z.enum([...Object.values(JOB_APPLICATION_STATUS)]),
}) satisfies z.ZodType<MoveJobApplicationStatusCommandCorrected>;

export const MarkHiredButton = ({ jobApplicationId }: Props) => {
    const moveJobApplicationStatusMutation = useMoveJobApplicationStatus();

    const form = useForm<z.infer<typeof moveJobApplicationStatusFormSchema>>({
        resolver: zodResolver(moveJobApplicationStatusFormSchema),
        defaultValues: {
            id: jobApplicationId,
            moveTo: JOB_APPLICATION_STATUS.HIRED,
        },
    });

    const onSubmit = async (data: MoveJobApplicationStatusCommandCorrected) => {
        moveJobApplicationStatusMutation.mutate(data);
    };

    return (
        <form onSubmit={form.handleSubmit(onSubmit)}>
            <Button
                variant="outline"
                type="submit"
                disabled={moveJobApplicationStatusMutation.isPending}
            >
                Mark Hired
            </Button>
        </form>
    );
};
