import { useMoveInterviewStatus } from '@/api/interviews-api';
import { Button } from '@/components/ui/button';
import { INTERVIEW_STATUS } from '@/types/enums';
import type { MoveInterviewStatusCommandCorrected } from '@/types/interview-types';
import { zodResolver } from '@hookform/resolvers/zod';
import { useForm } from 'react-hook-form';
import z from 'zod';

type Props = {
    interviewId: string;
};

const moveJobApplicationStatusFormSchema = z.object({
    interviewId: z.string(),
    moveTo: z.enum([...Object.values(INTERVIEW_STATUS)]),
}) satisfies z.ZodType<MoveInterviewStatusCommandCorrected>;

export const MarkCompletedButton = ({ interviewId }: Props) => {
    const moveInterviewStatusMutation = useMoveInterviewStatus();

    const form = useForm<z.infer<typeof moveJobApplicationStatusFormSchema>>({
        resolver: zodResolver(moveJobApplicationStatusFormSchema),
        defaultValues: {
            interviewId: interviewId,
            moveTo: INTERVIEW_STATUS.COMPLETED,
        },
    });

    const onSubmit = async (data: MoveInterviewStatusCommandCorrected) => {
        moveInterviewStatusMutation.mutate(data);
    };

    return (
        <form onSubmit={form.handleSubmit(onSubmit)}>
            <Button
                variant="outline"
                type="submit"
                disabled={moveInterviewStatusMutation.isPending}
            >
                Mark Completed
            </Button>
        </form>
    );
};
