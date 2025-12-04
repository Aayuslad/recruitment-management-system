import { useEditDesignation } from '@/api/designation-api';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { useAppStore } from '@/store';
import type { EditDesignationCommandCorrected } from '@/types/designation-types';
import { zodResolver } from '@hookform/resolvers/zod';
import { useEffect } from 'react';
import { useFieldArray, useForm } from 'react-hook-form';
import { toast } from 'sonner';
import z from 'zod';
import { useShallow } from 'zustand/react/shallow';
import { DesignationSkillEditor } from './designation-skill-editor';
import { DialogClose, DialogFooter } from '@/components/ui/dialog';
import { Button } from '@/components/ui/button';

const editDesignationFormSchema = z.object({
    id: z.string().nonempty('Designation ID is required'),
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
}) satisfies z.ZodType<EditDesignationCommandCorrected>;

export const EditDialogBody = () => {
    const editDesignationMutation = useEditDesignation();

    const { designationDialogTarget, closeDesignationDialog } = useAppStore(
        useShallow((s) => ({
            designationDialogTarget: s.designationDialogTarget,
            closeDesignationDialog: s.closeDesignationDialog,
        }))
    );

    // eslint-disable-next-line @typescript-eslint/no-unused-vars
    const { control, setValue, formState, handleSubmit, register, reset } =
        useForm<EditDesignationCommandCorrected>({
            resolver: zodResolver(editDesignationFormSchema),
        });

    const designationSkillsFealdArray = useFieldArray({
        control,
        name: 'designationSkills',
    });

    useEffect(() => {
        setValue('id', designationDialogTarget?.id || '');
        setValue('name', designationDialogTarget?.name || '');
        setValue(
            'designationSkills',
            designationDialogTarget?.designationSkills.map((x) => ({
                skillId: x.skillId,
                skillType: x.skillType,
                minExperienceYears: x.minExperienceYears,
            })) || []
        );
    }, [designationDialogTarget, setValue]);

    const onSubmit = async (data: EditDesignationCommandCorrected) => {
        editDesignationMutation.mutate(data, {
            onSuccess: () => {
                closeDesignationDialog();
                reset();
            },
        });
    };

    const onInvalid = (errors: typeof formState.errors) => {
        const messages = Object.values(errors).map((err) => err.message);
        messages.reverse().forEach((msg) => toast.error(msg));
    };

    return (
        <div>
            <form
                onSubmit={handleSubmit(onSubmit, onInvalid)}
                className="grid gap-4"
            >
                <div className="grid gap-4">
                    <div className="grid gap-3">
                        <Label htmlFor="name">Name</Label>
                        <Input
                            id="name"
                            className="w-[350px]"
                            {...register('name')}
                        />
                    </div>
                </div>

                <div className="grid gap-3">
                    <Label htmlFor="skills">Skills</Label>
                    <DesignationSkillEditor
                        fields={designationSkillsFealdArray.fields}
                        append={designationSkillsFealdArray.append}
                        remove={designationSkillsFealdArray.remove}
                        update={designationSkillsFealdArray.update}
                    />
                </div>

                <DialogFooter>
                    <DialogClose asChild>
                        <Button
                            variant="outline"
                            disabled={editDesignationMutation.isPending}
                        >
                            Cancel
                        </Button>
                    </DialogClose>
                    <Button
                        type="submit"
                        disabled={editDesignationMutation.isPending}
                    >
                        Save
                    </Button>
                </DialogFooter>
            </form>
        </div>
    );
};
