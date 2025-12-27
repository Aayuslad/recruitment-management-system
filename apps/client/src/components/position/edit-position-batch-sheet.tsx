import { useEditPositionBatch, useGetPositionBatch } from '@/api/position-api';
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
import type { EditPositionBatchCommandCorrected } from '@/types/position-types';
import { zodResolver } from '@hookform/resolvers/zod';
import { useEffect, useState } from 'react';
import { useFieldArray, useForm } from 'react-hook-form';
import { toast } from 'sonner';
import { z } from 'zod';
import { Textarea } from '../ui/textarea';
import { ReviewersSelector } from './internal/reviewers-selector';
import { PositionSkillSelector } from './internal/position-skill-selector';
import { Spinner } from '../ui/spinner';
import { useAccessChecker } from '@/hooks/use-has-access';

const editPositionBatchFormSchema = z.object({
    positionBatchId: z.string(),
    description: z.string().nullable().optional(),
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
            id: z.string().optional().nullable(),
            skillId: z.string(),
            comments: z.string().optional().nullable(),
            minExperienceYears: z.number(),
            type: z.enum(['Required', 'Preferred']),
            actionType: z.enum(['Add', 'Remove', 'Update']),
        })
    ),
}) satisfies z.ZodType<EditPositionBatchCommandCorrected>;

type Props = {
    visibleTo: string[];
    positionBatchId: string;
};

export function EditPositionBatchSheet({ positionBatchId, visibleTo }: Props) {
    const [open, setOpen] = useState(false);
    const editPositionBatchMutation = useEditPositionBatch();
    const canAccess = useAccessChecker();
    const {
        data: positionBatch,
        isLoading,
        isError,
    } = useGetPositionBatch(positionBatchId);

    const form = useForm<z.infer<typeof editPositionBatchFormSchema>>({
        resolver: zodResolver(editPositionBatchFormSchema),
    });

    const skillOverRidesFealdArray = useFieldArray({
        name: 'skillOverRides',
        control: form.control,
    });

    const reviewersFealdArray = useFieldArray({
        name: 'reviewers',
        control: form.control,
    });

    useEffect(() => {
        if (positionBatch) {
            form.reset(positionBatch);
            form.setValue('positionBatchId', positionBatchId);
        }
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [positionBatch]);

    const onSubmit = async (data: EditPositionBatchCommandCorrected) => {
        editPositionBatchMutation.mutate(data, {
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

    if (isLoading) {
        return (
            <div className="w-full h-[30vh] flex justify-center items-center">
                <Spinner className="size-8" />
            </div>
        );
    }

    if (isError) {
        return (
            <div className="w-full h-[30vh] flex justify-center items-center">
                Error Loading Data
            </div>
        );
    }

    if (!canAccess(visibleTo)) return null;

    return (
        <Sheet open={open} onOpenChange={setOpen}>
            <SheetTrigger asChild>
                <Button variant="outline" className="w-[160px]">
                    Edit Position Batch
                </Button>
            </SheetTrigger>
            <SheetContent className="w-[40vw]">
                <form
                    onSubmit={form.handleSubmit(onSubmit, onInvalid)}
                    className="flex h-full flex-col gap-4"
                >
                    <SheetHeader>
                        <SheetTitle>Edit Position Batch</SheetTitle>
                        <SheetDescription>
                            Edit deatils for your position batch. Click Save
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
                        <div>
                            <PositionSkillSelector
                                designationId={
                                    positionBatch?.designationId as string
                                }
                                skillOverRides={skillOverRidesFealdArray.fields}
                                append={skillOverRidesFealdArray.append}
                                remove={skillOverRidesFealdArray.remove}
                                update={skillOverRidesFealdArray.update}
                            />
                        </div>
                        <div className="grid gap-2">
                            <ReviewersSelector
                                fealds={reviewersFealdArray.fields}
                                append={reviewersFealdArray.append}
                                remove={reviewersFealdArray.remove}
                            />
                        </div>
                    </div>
                    <SheetFooter className="flex flex-row w-full">
                        <Button
                            type="submit"
                            className="flex-1"
                            disabled={editPositionBatchMutation.isPending}
                        >
                            Save
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
