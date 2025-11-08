import { useCreateDesignation } from '@/api/designation';
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
import type { CreateDesignationCommandCorrected } from '@/types/designation-types';
import { zodResolver } from '@hookform/resolvers/zod';
import { useEffect, useState } from 'react';
import { useForm } from 'react-hook-form';
import { toast } from 'sonner';
import z from 'zod';
import { SkillSelector } from './skill-selector';
import {
    Select,
    SelectContent,
    SelectItem,
    SelectTrigger,
    SelectValue,
} from '@/components/ui/select';
import { X } from 'lucide-react';

const createDesignationFormSchema = z.object({
    name: z
        .string()
        .min(2, 'Name must be at least 2 characters long')
        .max(30, 'Name must be at most 100 characters long'),
    description: z
        .string()
        .min(1, 'Description is required')
        .max(500, 'Description must be at most 500 characters long'),
    designationSkills: z
        .object({
            skillId: z.string(),
            skillType: z.enum(['Required', 'Preferred', 'NiceToHave']),
            minExperienceYears: z.number().min(0).max(50),
        })
        .array()
        .nullable(),
}) satisfies z.ZodType<CreateDesignationCommandCorrected>;

export function CreateDesignationDialog() {
    const [open, setOpen] = useState(false);
    const [skills, setSkills] = useState<{ id: string; label: string }[]>([]);
    const createDesignationMutation = useCreateDesignation();

    const form = useForm<CreateDesignationCommandCorrected>({
        resolver: zodResolver(createDesignationFormSchema),
        defaultValues: {
            name: '',
            description: '',
            designationSkills: [],
        },
    });

    useEffect(() => {
        form.setValue(
            'designationSkills',
            skills.map((skill) => ({
                skillId: skill.id,
                skillType: 'Required',
                minExperienceYears: 0,
            }))
        );
    }, [skills, form]);

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
                <Button variant="outline">Create Designation</Button>
            </DialogTrigger>

            <DialogContent className="sm:max-w-[425px]">
                <form
                    onSubmit={form.handleSubmit(onSubmit, onInvalid)}
                    className="grid gap-4"
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
                        <div className="grid gap-3">
                            <Label htmlFor="description">Description</Label>
                            <Textarea
                                id="description"
                                placeholder="Passout butter"
                                {...form.register('description')}
                            />
                        </div>

                        <div className="max-h-[220px] overflow-y-auto pr-2 custom-scroll">
                            {skills.map((skill, idx) => (
                                <div
                                    key={idx}
                                    className="flex items-center gap-2 mb-2"
                                >
                                    <Input
                                        value={skill.label}
                                        disabled
                                        className="w-[120px] h-8 text-sm px-2"
                                    />
                                    <Select>
                                        <SelectTrigger className="w-[120px] h-8 text-sm px-2">
                                            <SelectValue placeholder="Type" />
                                        </SelectTrigger>
                                        <SelectContent className="text-sm">
                                            <SelectItem value="required">
                                                Required
                                            </SelectItem>
                                            <SelectItem value="preferred">
                                                Preferred
                                            </SelectItem>
                                            <SelectItem value="nice">
                                                Nice to Have
                                            </SelectItem>
                                        </SelectContent>
                                    </Select>
                                    <div className="flex items-center gap-1">
                                        <Label
                                            htmlFor="exp"
                                            className="text-xs"
                                        >
                                            Exp
                                        </Label>
                                        <Input
                                            type="number"
                                            placeholder="0"
                                            className="w-12 h-8 text-sm px-2"
                                        />
                                    </div>
                                    <Button
                                        variant="ghost"
                                        size="icon"
                                        className="h-8 w-8 text-muted-foreground"
                                        // onClick={() => removeSkill(idx)}
                                    >
                                        <X className="h-4 w-4" />
                                    </Button>
                                </div>
                            ))}
                        </div>

                        <div className="grid gap-3">
                            <Label htmlFor="skills">Skills</Label>
                            <SkillSelector
                                skills={skills}
                                setSkills={setSkills}
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
