import { useEditJobOpening, useGetJobOpening } from '@/api/job-opening-api';
import { Button } from '@/components/ui/button';
import {
    Sheet,
    SheetClose,
    SheetContent,
    SheetDescription,
    SheetFooter,
    SheetHeader,
    SheetTitle,
    SheetTrigger,
} from '@/components/ui/sheet';
import type { EditJobOpeningCommandCorrected } from '@/types/job-opening-types';
import { useEffect, useState } from 'react';
import { useFieldArray, useForm } from 'react-hook-form';
import z from 'zod';
import { zodResolver } from '@hookform/resolvers/zod';
import { toast } from 'sonner';
import { Label } from '../ui/label';
import { Input } from '../ui/input';
import { TypeSelector } from './internal/job-opening-type-selector';
import { Textarea } from '../ui/textarea';
import { JobOpeningSkillSelector } from './internal/job-opening-skill-selector';
import { InterviewRoundSelector } from './internal/interview-round-selector';
import { InterviewParticipantSelector } from './internal/interview-participant-selector';
import { Spinner } from '../ui/spinner';
import { useAccessChecker } from '@/hooks/use-has-access';

const EditJobOpeningSheetSchema = z.object({
    jobOpeningId: z.string(),
    title: z.string(),
    description: z.string().optional().nullable(),
    type: z.enum(['Normal', 'CampusDrive', 'WalkIn']),
    positionBatchId: z.string(),
    interviewers: z.array(
        z.object({
            userId: z.string(),
            role: z.enum([
                'TechnicalInterviewer',
                'Observer',
                'NoteTaker',
                'HRInterviewer',
            ]),
        })
    ),
    interviewRounds: z.array(
        z.object({
            id: z.string().optional().nullable(),
            type: z.enum(['Technical', 'HR', 'OnlineTest']),
            roundNumber: z.number(),
            durationInMinutes: z.number(),
            requirements: z.array(
                z.object({
                    id: z.string().optional().nullable(),
                    role: z.enum([
                        'TechnicalInterviewer',
                        'Observer',
                        'NoteTaker',
                        'HRInterviewer',
                    ]),
                    requirementCount: z.number(),
                })
            ),
        })
    ),
    skillOverRides: z.array(
        z.object({
            id: z.string().optional().nullable(),
            skillId: z.string(),
            comments: z.string().optional().nullable(),
            type: z.enum(['Required', 'Preferred']),
            actionType: z.enum(['Add', 'Remove', 'Update']),
        })
    ),
}) satisfies z.ZodType<EditJobOpeningCommandCorrected>;

type Props = {
    jobOpeningId: string;
    visibleTo: string[];
};

export function EditJobOpeningSheet({ jobOpeningId, visibleTo }: Props) {
    const [open, setOpen] = useState(false);
    const editJobOpeningMutation = useEditJobOpening();
    const canAccess = useAccessChecker();
    const { data, isLoading, isError } = useGetJobOpening(jobOpeningId);

    const form = useForm<z.infer<typeof EditJobOpeningSheetSchema>>({
        resolver: zodResolver(EditJobOpeningSheetSchema),
    });

    useEffect(() => {
        if (data) {
            form.reset(data);
            form.setValue('jobOpeningId', jobOpeningId);
            form.setValue(
                'interviewRounds',
                data.interviewRounds.sort(
                    (a, b) => a.roundNumber - b.roundNumber
                )
            );
        }
    }, [data, form, jobOpeningId]);

    const skillOverRidesFieldArray = useFieldArray({
        name: 'skillOverRides',
        control: form.control,
    });

    const interviewRoundsFieldArray = useFieldArray({
        name: 'interviewRounds',
        control: form.control,
    });

    const interviewersFieldArray = useFieldArray({
        name: 'interviewers',
        control: form.control,
    });

    const onSubmit = async (data: EditJobOpeningCommandCorrected) => {
        editJobOpeningMutation.mutate(data, {
            onSuccess: () => {
                form.reset();
                setOpen(false);
            },
        });
    };

    const onInvalid = (errors: typeof form.formState.errors) => {
        const messages = Object.values(errors).map((err) => err.message);
        messages.reverse().forEach((msg) => toast.error(msg));
    };

    if (!canAccess(visibleTo)) return null;

    if (isLoading)
        return (
            <div className="h-[50vh] flex justify-center items-center">
                <Spinner className="size-8" />
            </div>
        );

    if (isError) return <div>Error Loading Job Opening</div>;

    return (
        <Sheet open={open} onOpenChange={setOpen}>
            <SheetTrigger asChild>
                <Button variant="outline" className="w-[160px]">
                    Edit Job Opening
                </Button>
            </SheetTrigger>
            <SheetContent className="w-[40vw]">
                <form
                    className="flex h-full flex-col gap-4"
                    onSubmit={form.handleSubmit(onSubmit, onInvalid)}
                >
                    <SheetHeader>
                        <SheetTitle>Edit Job Opening</SheetTitle>
                        <SheetDescription>
                            Edit details for your job opening. Click Save when
                            you&apos;re done.
                        </SheetDescription>
                    </SheetHeader>

                    <div
                        className="flex-1 grid auto-rows-min gap-6 px-4 overflow-y-auto py-1"
                        style={{
                            maxHeight:
                                'calc(100vh - var(--sheet-header-height))',
                        }}
                    >
                        <div className="grid gap-2">
                            <Label htmlFor="title">Title</Label>
                            <Input
                                className="w-[300px]"
                                id="title"
                                placeholder="#1 Job Opening for 2026 !"
                                {...form.register('title')}
                            />
                        </div>

                        <div className="grid gap-2">
                            <Label htmlFor="type">Type</Label>
                            <TypeSelector
                                selectedType={form.watch('type')}
                                setSelectedType={(type) =>
                                    form.setValue('type', type)
                                }
                            />
                        </div>
                        <div className="grid gap-2">
                            <Label htmlFor="description">Description</Label>
                            <Textarea
                                id="description"
                                placeholder="some description...."
                                {...form.register('description')}
                            />
                        </div>
                        <div className="grid gap-2">
                            <JobOpeningSkillSelector
                                positionBatchId={form.watch('positionBatchId')}
                                skillOverRides={skillOverRidesFieldArray.fields}
                                append={skillOverRidesFieldArray.append}
                                remove={skillOverRidesFieldArray.remove}
                                update={skillOverRidesFieldArray.update}
                            />
                        </div>

                        <div className="my-5">
                            <div className="flex items-center justify-between gap-2">
                                <div className="border-b flex-1"></div>
                                <div>Interview Template (optional)</div>
                                <div className="border-b flex-1"></div>
                            </div>
                            <div className="text-muted-foreground text-center">
                                If you want interviews to be created
                                automatically when a candidate is shortlisted,
                                you can define an interview template here.
                            </div>
                        </div>

                        <div className="grid gap-2">
                            <InterviewRoundSelector
                                fields={interviewRoundsFieldArray.fields}
                                append={interviewRoundsFieldArray.append}
                                remove={interviewRoundsFieldArray.remove}
                                update={interviewRoundsFieldArray.update}
                            />
                        </div>

                        <div className="grid gap-2">
                            <InterviewParticipantSelector
                                fields={interviewersFieldArray.fields}
                                append={interviewersFieldArray.append}
                                remove={interviewersFieldArray.remove}
                                interviewRounds={
                                    form.watch('interviewRounds') ?? []
                                }
                            />
                        </div>
                    </div>

                    <SheetFooter className="flex flex-row w-full">
                        <Button
                            type="submit"
                            className="flex-1"
                            disabled={editJobOpeningMutation.isPending}
                        >
                            Save
                        </Button>
                        <SheetClose className="flex-1" asChild>
                            <Button
                                variant="outline"
                                disabled={editJobOpeningMutation.isPending}
                            >
                                Close
                            </Button>
                        </SheetClose>
                    </SheetFooter>
                </form>
            </SheetContent>
        </Sheet>
    );
}
