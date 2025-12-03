import { useEditSkill } from '@/api/skill-api';
import { Button } from '@/components/ui/button';
import {
    Dialog,
    DialogClose,
    DialogContent,
    DialogDescription,
    DialogFooter,
    DialogHeader,
    DialogTitle,
} from '@/components/ui/dialog';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { useAppStore } from '@/store';
import type { EditSkillCommandCorrected } from '@/types/skill-types';
import { zodResolver } from '@hookform/resolvers/zod';
import { useEffect } from 'react';
import { useForm } from 'react-hook-form';
import { toast } from 'sonner';
import z from 'zod';
import { useShallow } from 'zustand/react/shallow';

const editSkillFormSchema = z.object({
    id: z.string().nonempty('Skill ID is required'),
    name: z
        .string()
        .min(2, 'Name must be at least 2 characters long')
        .max(50, 'Name must be at most 50 characters long'),
}) satisfies z.ZodType<EditSkillCommandCorrected>;

export function EditSkillDialog() {
    const {
        skillEditTarget,
        skillEditDialog,
        closeSkillEditDialog,
        setSkillEditDialog,
    } = useAppStore(
        useShallow((s) => ({
            skillEditTarget: s.skillEditTarget,
            skillEditDialog: s.skillEditDialog,
            closeSkillEditDialog: s.closeSkillEditDialog,
            setSkillEditDialog: s.setSkillEditDialog,
        }))
    );

    const editSkillMutation = useEditSkill();

    const form = useForm<EditSkillCommandCorrected>({
        resolver: zodResolver(editSkillFormSchema),
    });

    useEffect(() => {
        form.setValue('id', skillEditTarget?.id || '');
        form.setValue('name', skillEditTarget?.name || '');
    }, [skillEditTarget, form]);

    const onSubmit = async (data: EditSkillCommandCorrected) => {
        editSkillMutation.mutate(data, {
            onSuccess: () => {
                form.reset();
                closeSkillEditDialog();
            },
        });
    };

    const onInvalid = (errors: typeof form.formState.errors) => {
        const messages = Object.values(errors).map((err) => err.message);
        messages.reverse().forEach((msg) => toast.error(msg));
    };

    return (
        <Dialog open={skillEditDialog} onOpenChange={setSkillEditDialog}>
            <DialogContent className="sm:max-w-[425px]">
                <form
                    onSubmit={form.handleSubmit(onSubmit, onInvalid)}
                    className="grid gap-7"
                >
                    <DialogHeader>
                        <DialogTitle>Edit Skill</DialogTitle>
                        <DialogDescription>
                            Edit the skill details and click Save.
                        </DialogDescription>
                    </DialogHeader>

                    <div className="grid gap-4">
                        <div className="grid gap-3">
                            <Label htmlFor="name">Name</Label>
                            <Input id="name" {...form.register('name')} />
                        </div>
                    </div>

                    <DialogFooter>
                        <DialogClose asChild>
                            <Button
                                variant="outline"
                                disabled={editSkillMutation.isPending}
                            >
                                Cancel
                            </Button>
                        </DialogClose>
                        <Button
                            type="submit"
                            disabled={editSkillMutation.isPending}
                        >
                            Save
                        </Button>
                    </DialogFooter>
                </form>
            </DialogContent>
        </Dialog>
    );
}
