import { useCreateJobApplicationFeedback } from '@/api/job-application-api';
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
import { Textarea } from '@/components/ui/textarea';
import type { CreateJobApplicationFeedbackCommandCorrected } from '@/types/job-application-types';
import { zodResolver } from '@hookform/resolvers/zod';
import { useState } from 'react';
import { useFieldArray, useForm } from 'react-hook-form';
import { toast } from 'sonner';
import z from 'zod';
import { SkillFeedbackInput } from './skill-feedback-intput';

type Props = {
    jobApplicationId?: string;
    candidateId?: string;
};

const createFeedbackFormSchema = z.object({
    jobApplicationId: z.string(),
    comment: z.string().min(2, 'Comment must be at least 2 characters long'),
    rating: z.number(),
    skillFeedbacks: z.array(
        z.object({
            skillId: z.string(),
            rating: z.number(),
            assessedExpYears: z.number().optional().nullable(),
        })
    ),
}) satisfies z.ZodType<CreateJobApplicationFeedbackCommandCorrected>;

export function AddFeedbackDialog({ jobApplicationId, candidateId }: Props) {
    const [open, setOpen] = useState(false);
    const createFeedbackMutation = useCreateJobApplicationFeedback();

    const form = useForm<z.infer<typeof createFeedbackFormSchema>>({
        resolver: zodResolver(createFeedbackFormSchema),
        defaultValues: {
            jobApplicationId: jobApplicationId,
            comment: '',
            rating: 0,
            skillFeedbacks: [],
        },
    });

    const skillFeedbackFieldArray = useFieldArray({
        control: form.control,
        name: 'skillFeedbacks',
    });

    const onSubmit = async (
        data: CreateJobApplicationFeedbackCommandCorrected
    ) => {
        createFeedbackMutation.mutate(data, {
            onSuccess: () => {
                toast.success('Feedback submitted');
                form.reset();
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
                <Button variant={'ghost'} className="h-8">
                    + Give Feedback
                </Button>
            </DialogTrigger>
            <DialogContent className="sm:max-w-[550px]">
                <form
                    onSubmit={form.handleSubmit(onSubmit, onInvalid)}
                    className="space-y-4"
                >
                    <DialogHeader>
                        <DialogTitle>Feedback</DialogTitle>
                        <DialogDescription>
                            Give your feedback here. Click submit when
                            you&apos;re done.
                        </DialogDescription>
                    </DialogHeader>
                    <div className="grid gap-4">
                        <div className="grid gap-3">
                            <Label htmlFor="name-1">Rating</Label>
                            <Input
                                id="name-1"
                                type="number"
                                min={0}
                                max={10}
                                className="w-[100px]"
                                {...form.register('rating', {
                                    valueAsNumber: true,
                                })}
                            />
                        </div>
                        <div className="grid gap-3">
                            <Label htmlFor="comment">Comment</Label>
                            <Textarea
                                id="comment"
                                {...form.register('comment')}
                                placeholder="Add comments..."
                            />
                        </div>

                        <div className="grid gap-3">
                            <SkillFeedbackInput
                                candidateId={candidateId}
                                skillFeedbacks={skillFeedbackFieldArray.fields}
                                append={skillFeedbackFieldArray.append}
                                remove={skillFeedbackFieldArray.remove}
                                update={skillFeedbackFieldArray.update}
                            />
                        </div>
                    </div>

                    <DialogFooter>
                        <DialogClose asChild>
                            <Button
                                variant="outline"
                                disabled={createFeedbackMutation.isPending}
                            >
                                Cancel
                            </Button>
                        </DialogClose>
                        <Button
                            type="submit"
                            disabled={createFeedbackMutation.isPending}
                        >
                            Submit
                        </Button>
                    </DialogFooter>
                </form>
            </DialogContent>
        </Dialog>
    );
}
