import { useEditUserRoles } from '@/api/user-api';
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
import { useAppStore } from '@/store';
import type { EditUserRolesCommandCorrected } from '@/types/user-types';
import { zodResolver } from '@hookform/resolvers/zod';
import { useEffect } from 'react';
import { useFieldArray, useForm } from 'react-hook-form';
import { toast } from 'sonner';
import z from 'zod';
import { useShallow } from 'zustand/react/shallow';
import { UserRolesSelector } from './internal/user-roles-selector';

const editUserRolesFormSchema = z.object({
    userId: z.string().nonempty('User ID is required'),
    roles: z.array(
        z.object({
            roleId: z.string().nonempty('Role ID is required'),
            assignedBy: z.string().optional().nullable(),
        })
    ),
}) satisfies z.ZodType<EditUserRolesCommandCorrected>;

export function ManageUserRolesDialog() {
    const {
        userEditTarget,
        userEditDialog,
        closeUserEditDialog,
        setUserEditDialog,
    } = useAppStore(
        useShallow((s) => ({
            userEditTarget: s.userEditTarget,
            userEditDialog: s.userEditDialog,
            closeUserEditDialog: s.closeUserEditDialog,
            setUserEditDialog: s.setUserEditDialog,
        }))
    );

    const editUserRolesMutation = useEditUserRoles();

    const form = useForm<EditUserRolesCommandCorrected>({
        resolver: zodResolver(editUserRolesFormSchema),
    });

    const rolesFieldArray = useFieldArray({
        control: form.control,
        name: 'roles',
    });

    useEffect(() => {
        if (userEditTarget) {
            form.setValue('userId', userEditTarget.userId);
            form.setValue(
                'roles',
                userEditTarget.roles.map((role) => ({
                    roleId: role.id,
                    assignedBy: role.assignedBy,
                }))
            );
        }
    }, [userEditTarget, form]);

    const onSubmit = async (data: EditUserRolesCommandCorrected) => {
        editUserRolesMutation.mutate(data, {
            onSuccess: () => {
                form.reset();
                closeUserEditDialog();
            },
        });
    };

    const onInvalid = (errors: typeof form.formState.errors) => {
        const messages = Object.values(errors).map((err) => err.message);
        messages.reverse().forEach((msg) => toast.error(msg));
    };

    return (
        <Dialog open={userEditDialog} onOpenChange={setUserEditDialog}>
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
                        <UserRolesSelector
                            userRoles={rolesFieldArray.fields}
                            appendedRoles={rolesFieldArray.append}
                            removedRoles={rolesFieldArray.remove}
                        />
                    </div>

                    <DialogFooter>
                        <DialogClose asChild>
                            <Button
                                variant="outline"
                                disabled={editUserRolesMutation.isPending}
                            >
                                Cancel
                            </Button>
                        </DialogClose>
                        <Button
                            type="submit"
                            disabled={editUserRolesMutation.isPending}
                        >
                            Save
                        </Button>
                    </DialogFooter>
                </form>
            </DialogContent>
        </Dialog>
    );
}
