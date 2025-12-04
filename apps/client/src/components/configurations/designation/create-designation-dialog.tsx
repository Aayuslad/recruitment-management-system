import { useCreateDesignation } from '@/api/designation-api';
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
import type { CreateDesignationCommandCorrected } from '@/types/designation-types';
import { zodResolver } from '@hookform/resolvers/zod';
import { useState } from 'react';
import { useForm } from 'react-hook-form';
import { toast } from 'sonner';
import z from 'zod';

const createDesignationFormSchema = z.object({
    name: z
        .string()
        .min(2, 'Name must be at least 2 characters long')
        .max(50, 'Name must be at most 50 characters long'),
    designationSkills: z
        .object({
            skillId: z.string(),
            skillType: z.enum(['Required', 'Preferred', 'NiceToHave']),
            minExperienceYears: z.number().min(0).max(50),
        })
        .array(),
}) satisfies z.ZodType<CreateDesignationCommandCorrected>;

export function CreateDesignationDialog() {
    const [open, setOpen] = useState(false);
    const createDesignationMutation = useCreateDesignation();

    const form = useForm<CreateDesignationCommandCorrected>({
        resolver: zodResolver(createDesignationFormSchema),
        defaultValues: {
            name: '',
            designationSkills: [],
        },
    });

    const onSubmit = async (data: CreateDesignationCommandCorrected) => {
        createDesignationMutation.mutate(data, {
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
        <Dialog open={open} onOpenChange={setOpen}>
            <DialogTrigger asChild>
                <Button variant="secondary" className="border">
                    Create Designation
                </Button>
            </DialogTrigger>

            <DialogContent className="sm:max-w-[425px]">
                <form
                    onSubmit={form.handleSubmit(onSubmit, onInvalid)}
                    className="grid gap-7"
                >
                    <DialogHeader>
                        <DialogTitle>Create Designation</DialogTitle>
                        <DialogDescription>
                            Enter the designation details and click Create.
                        </DialogDescription>
                    </DialogHeader>

                    <div className="grid gap-4">
                        <div className="grid gap-3">
                            <Label htmlFor="name">Name</Label>
                            <Input
                                id="name"
                                placeholder="SDE 1"
                                {...form.register('name')}
                            />
                        </div>
                    </div>

                    <DialogFooter>
                        <DialogClose asChild>
                            <Button
                                variant="outline"
                                disabled={createDesignationMutation.isPending}
                            >
                                Cancel
                            </Button>
                        </DialogClose>
                        <Button
                            type="submit"
                            disabled={createDesignationMutation.isPending}
                        >
                            Create
                        </Button>
                    </DialogFooter>
                </form>
            </DialogContent>
        </Dialog>
    );
}
