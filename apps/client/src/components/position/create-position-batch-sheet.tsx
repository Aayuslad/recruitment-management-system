import { useCreatePositionBatch } from '@/api/position-api';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
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
import type { CreatePositionBatchCommandCorrected } from '@/types/position-types';
import { zodResolver } from '@hookform/resolvers/zod';
import { useState } from 'react';
import { useFieldArray, useForm } from 'react-hook-form';
import { toast } from 'sonner';
import { z } from 'zod';
import { Textarea } from '../ui/textarea';
import { DesignationSelector } from './internal/designation-selector';
import { PositionSkillSelector } from './internal/position-skill-selector';
import { ReviewersSelector } from './internal/reviewers-selector';
import { useAccessChecker } from '@/hooks/use-has-access';

const createPositionBatchFormSchema = z.object({
    numberOfPositions: z.number(),
    description: z.string().nullable().optional(),
    designationId: z.string(),
    minCTC: z.number(),
    maxCTC: z.number(),
    jobLocation: z.string(),
    reviewers: z.array(
        z.object({
            reviewerUserId: z.string(),
        })
    ),
    skillOverRides: z.array(
        z.object({
            skillId: z.string(),
            comments: z.string().optional().nullable(),
            type: z.enum(['Required', 'Preferred']),
            actionType: z.enum(['Add', 'Remove', 'Update']),
        })
    ),
}) satisfies z.ZodType<CreatePositionBatchCommandCorrected>;

type Props = {
    visibleTo: string[];
};

export function CreatePositionBatchSheet({ visibleTo }: Props) {
    const [open, setOpen] = useState(false);
    const canAccess = useAccessChecker();
    const createPositionBatchMutation = useCreatePositionBatch();

    const form = useForm<z.infer<typeof createPositionBatchFormSchema>>({
        resolver: zodResolver(createPositionBatchFormSchema),
    });

    const skillOverRidesFieldArray = useFieldArray({
        name: 'skillOverRides',
        control: form.control,
    });

    const reviewersFieldArray = useFieldArray({
        name: 'reviewers',
        control: form.control,
    });

    const onSubmit = async (data: CreatePositionBatchCommandCorrected) => {
        createPositionBatchMutation.mutate(data, {
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

    return (
        <Sheet open={open} onOpenChange={setOpen}>
            <SheetTrigger asChild>
                <Button variant="secondary" className="border">
                    + Create Position Batch
                </Button>
            </SheetTrigger>
            <SheetContent className="w-[35vw]">
                <form
                    onSubmit={form.handleSubmit(onSubmit, onInvalid)}
                    className="flex h-full flex-col gap-4"
                >
                    <SheetHeader>
                        <SheetTitle>Create Position Batch</SheetTitle>
                        <SheetDescription>
                            Add details for your position batch. Click create
                            when you&apos;re done.
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
                            <Label htmlFor="designation">Designation</Label>
                            <DesignationSelector
                                setSelectedDesignationId={(id) =>
                                    form.setValue('designationId', id)
                                }
                            />
                        </div>
                        <div className="grid gap-2">
                            <Label htmlFor="location">Location</Label>
                            <Input
                                className="w-[300px]"
                                id="location"
                                placeholder="location of jon position"
                                {...form.register('jobLocation')}
                            />
                        </div>
                        <div className="grid gap-2">
                            <Label htmlFor="ctc-range">CTC Range</Label>
                            <div className="flex flex-row gap-2 items-center">
                                <Input
                                    type="number"
                                    id="description"
                                    placeholder="min"
                                    className="w-[90px]"
                                    {...form.register('minCTC', {
                                        valueAsNumber: true,
                                    })}
                                />
                                <span>-</span>
                                <Input
                                    type="number"
                                    id="description"
                                    placeholder="max"
                                    className="w-[90px]"
                                    {...form.register('maxCTC', {
                                        valueAsNumber: true,
                                    })}
                                />
                                <span className="ml-2">LPA</span>
                            </div>
                        </div>
                        <div className="grid gap-2">
                            <Label htmlFor="description">Description</Label>
                            <Textarea
                                id="description"
                                placeholder="Position Batch Description"
                                {...form.register('description')}
                            />
                        </div>
                        <div className="grid gap-2">
                            <Label htmlFor="number-of-positions">
                                Number of position
                            </Label>
                            <Input
                                type="number"
                                className="w-[90px]"
                                id="number-of-positions"
                                placeholder="ex: 10"
                                {...form.register('numberOfPositions', {
                                    valueAsNumber: true,
                                })}
                            />
                        </div>
                        <div>
                            <PositionSkillSelector
                                designationId={form.watch('designationId')}
                                skillOverRides={skillOverRidesFieldArray.fields}
                                append={skillOverRidesFieldArray.append}
                                remove={skillOverRidesFieldArray.remove}
                                update={skillOverRidesFieldArray.update}
                            />
                        </div>
                        <div className="grid gap-2">
                            <ReviewersSelector
                                fields={reviewersFieldArray.fields}
                                append={reviewersFieldArray.append}
                                remove={reviewersFieldArray.remove}
                            />
                        </div>
                    </div>
                    <SheetFooter className="flex flex-row w-full">
                        <Button
                            type="submit"
                            className="flex-1"
                            disabled={createPositionBatchMutation.isPending}
                        >
                            Create Batch
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
