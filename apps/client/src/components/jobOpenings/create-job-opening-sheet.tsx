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
import { useState } from 'react';
import { Label } from '../ui/label';
import { Input } from '../ui/input';
import { useCreateJobOpening } from '@/api/job-opening-api';
import { useFieldArray, useForm } from 'react-hook-form';
import z from 'zod';
import { zodResolver } from '@hookform/resolvers/zod';
import type { CreateJobOpeningCommandCorrected } from '@/types/job-opening-types';
import { toast } from 'sonner';
import { JobOpeningSkillSelector } from './internal/job-opening-skill-selector';
import { PositionBatchSelector } from './internal/position-batch-selector';
import { TypeSelector } from './internal/job-opening-type-selector';
import { Textarea } from '../ui/textarea';
import { InterviewRoundSelector } from './internal/interview-round-selector';
import { InterviewParticipantSelector } from './internal/interview-participant-selector';

const CreateJobOpeningSheetSchema = z.object({
    title: z.string(),
    description: z.string().optional().nullable(),
    type: z.enum(['Normal', 'CampusDrive', 'WalkIn']),
    positionBatchId: z.string(),
    interviewers: z.array(
        z.object({
            userId: z.string(),
            role: z.enum([
                'Interviewer',
                'Observer',
                'NoteTaker',
                'HRRepresentative',
                'HiringManager',
            ]),
        })
    ),
    interviewRounds: z.array(
        z.object({
            type: z.enum(['Technical', 'HR', 'OnlineTest']),
            roundNumber: z.number(),
            durationInMinutes: z.number(),
            requirements: z.array(
                z.object({
                    role: z.enum([
                        'Interviewer',
                        'Observer',
                        'NoteTaker',
                        'HRRepresentative',
                        'HiringManager',
                    ]),
                    requirementCount: z.number(),
                })
            ),
        })
    ),
    skillOverRides: z.array(
        z.object({
            skillId: z.string(),
            comments: z.string().optional().nullable(),
            minExperienceYears: z.number(),
            type: z.enum(['Required', 'Preferred', 'NiceToHave']),
            actionType: z.enum(['Add', 'Remove', 'Update']),
        })
    ),
}) satisfies z.ZodType<CreateJobOpeningCommandCorrected>;

export function CreateJobOpeningSheet() {
    const [open, setOpen] = useState(false);
    const createJobOpeningMutation = useCreateJobOpening();

    const form = useForm<z.infer<typeof CreateJobOpeningSheetSchema>>({
        resolver: zodResolver(CreateJobOpeningSheetSchema),
    });

    const skillOverRidesFealdArray = useFieldArray({
        name: 'skillOverRides',
        control: form.control,
    });

    const interviewRoundsFealdArray = useFieldArray({
        name: 'interviewRounds',
        control: form.control,
    });

    const interviewersFealdArray = useFieldArray({
        name: 'interviewers',
        control: form.control,
    });

    const onSubmit = async (data: CreateJobOpeningCommandCorrected) => {
        createJobOpeningMutation.mutate(data, {
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

    return (
        <Sheet open={open} onOpenChange={setOpen}>
            <SheetTrigger asChild>
                <Button variant="secondary" className="border">
                    + Create Job Opening
                </Button>
            </SheetTrigger>
            <SheetContent className="w-[40vw]">
                <form
                    className="flex h-full flex-col gap-4"
                    onSubmit={form.handleSubmit(onSubmit, onInvalid)}
                >
                    <SheetHeader>
                        <SheetTitle>Create Job Opening</SheetTitle>
                        <SheetDescription>
                            Add deatils for your job opening. Click create when
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
                            <Label htmlFor="position-batch">
                                Position Batch
                            </Label>
                            <PositionBatchSelector
                                setSelectedPositionBatchId={(id) =>
                                    form.setValue('positionBatchId', id)
                                }
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
                                skillOverRides={skillOverRidesFealdArray.fields}
                                append={skillOverRidesFealdArray.append}
                                remove={skillOverRidesFealdArray.remove}
                                update={skillOverRidesFealdArray.update}
                            />
                        </div>

                        <div className="grid gap-2">
                            <InterviewRoundSelector
                                fealds={interviewRoundsFealdArray.fields}
                                append={interviewRoundsFealdArray.append}
                                remove={interviewRoundsFealdArray.remove}
                                update={interviewRoundsFealdArray.update}
                            />
                        </div>

                        <div className="grid gap-2">
                            <InterviewParticipantSelector
                                fealds={interviewersFealdArray.fields}
                                append={interviewersFealdArray.append}
                                remove={interviewersFealdArray.remove}
                            />
                        </div>
                    </div>

                    <SheetFooter className="flex flex-row w-full">
                        <Button type="submit" className="flex-1">
                            Create Opening
                        </Button>
                        <SheetClose className="flex-1" asChild>
                            <Button variant="outline">Close</Button>
                        </SheetClose>
                    </SheetFooter>
                </form>
            </SheetContent>
        </Sheet>
    );
}
