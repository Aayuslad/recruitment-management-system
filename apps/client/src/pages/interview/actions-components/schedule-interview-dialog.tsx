import { useMoveInterviewStatus } from '@/api/interviews-api';
import { Button } from '@/components/ui/button';
import {
    Dialog,
    DialogClose,
    DialogContent,
    DialogDescription,
    DialogFooter,
    DialogHeader,
    DialogTitle,
    DialogTrigger,
} from '@/components/ui/dialog';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { INTERVIEW_STATUS } from '@/types/enums';
import type { MoveInterviewStatusCommandCorrected } from '@/types/interview-types';
import { zodResolver } from '@hookform/resolvers/zod';
import { useState } from 'react';
import { useForm } from 'react-hook-form';
import { toast } from 'sonner';
import z from 'zod';

type Props = {
    interviewId: string;
};

const moveJobApplicationStatusFormSchema = z.object({
    interviewId: z.string(),
    moveTo: z.enum([...Object.values(INTERVIEW_STATUS)]),
    scheduledAt: z.date(),
    meetingLink: z.string(),
}) satisfies z.ZodType<MoveInterviewStatusCommandCorrected>;

export function ScheduleInterviewDialog({ interviewId }: Props) {
    const [open, setOpen] = useState(false);
    const moveInterviewStatusMutation = useMoveInterviewStatus();

    const form = useForm<z.infer<typeof moveJobApplicationStatusFormSchema>>({
        resolver: zodResolver(moveJobApplicationStatusFormSchema),
        defaultValues: {
            interviewId: interviewId,
            moveTo: INTERVIEW_STATUS.SCHEDULED,
        },
    });

    const onSubmit = async (data: MoveInterviewStatusCommandCorrected) => {
        moveInterviewStatusMutation.mutate(data, {
            onSuccess: () => {
                setOpen(false);
            },
        });
    };

    const onInvalid = (errors: typeof form.formState.errors) => {
        const messages = Object.values(errors).map((err) => err.message);
        messages.reverse().forEach((msg) => toast.error(msg));
    };

    return (
        <Dialog open={open} onOpenChange={setOpen}>
            <DialogTrigger asChild>
                <Button
                    variant="outline"
                    disabled={moveInterviewStatusMutation.isPending}
                >
                    Schedule Interview
                </Button>
            </DialogTrigger>
            <DialogContent className="sm:max-w-[425px]">
                <form
                    onSubmit={form.handleSubmit(onSubmit, onInvalid)}
                    className="space-y-6"
                >
                    <DialogHeader>
                        <DialogTitle>Schedule Interview</DialogTitle>
                        <DialogDescription>
                            Add time and meeting data here. Click schedule when
                            you&apos;re done.
                        </DialogDescription>
                    </DialogHeader>
                    <div className="grid gap-4">
                        <div className="grid gap-3">
                            <Label htmlFor="meeting-link">Meeting Link</Label>
                            <Input
                                id="meeting-link"
                                {...form.register('meetingLink')}
                            />
                        </div>
                        <div className="grid gap-3">
                            <Label htmlFor="meeting-link">Schedule At</Label>
                            <Input
                                type="datetime-local"
                                {...form.register('scheduledAt', {
                                    valueAsDate: true,
                                })}
                            />
                        </div>
                    </div>
                    <DialogFooter>
                        <DialogClose asChild>
                            <Button
                                variant="outline"
                                disabled={moveInterviewStatusMutation.isPending}
                            >
                                Cancel
                            </Button>
                        </DialogClose>
                        <Button
                            type="submit"
                            disabled={moveInterviewStatusMutation.isPending}
                        >
                            Schedule
                        </Button>
                    </DialogFooter>
                </form>
            </DialogContent>
        </Dialog>
    );
}
