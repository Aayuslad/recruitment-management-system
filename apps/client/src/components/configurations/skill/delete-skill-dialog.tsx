import { useDeleteSkill } from '@/api/skill-api';
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
import { useShallow } from 'zustand/react/shallow';

export function DeleteSkillDialog() {
    const {
        skillDeleteTargetId,
        skillDeleteDialog,
        closeSkillDeleteDialog,
        setSkillDeleteDialog,
    } = useAppStore(
        useShallow((s) => ({
            skillDeleteTargetId: s.skillDeleteTargetId,
            skillDeleteDialog: s.skillDeleteDialog,
            closeSkillDeleteDialog: s.closeSkillDeleteDialog,
            setSkillDeleteDialog: s.setSkillDeleteDialog,
        }))
    );
    const deleteSkillMutation = useDeleteSkill();

    const submit = async (id: string) => {
        deleteSkillMutation.mutate(id, {
            onSuccess: () => {
                closeSkillDeleteDialog();
            },
        });
    };

    return (
        <Dialog open={skillDeleteDialog} onOpenChange={setSkillDeleteDialog}>
            <DialogContent className="sm:max-w-[425px] space-y-2">
                <DialogHeader className="space-y-2">
                    <DialogTitle>Confirm Delete Skill</DialogTitle>
                    <DialogDescription>
                        This skill will be removed and this action cannot be
                        undone.
                    </DialogDescription>
                </DialogHeader>

                <DialogFooter>
                    <DialogClose asChild className="flex-1">
                        <Button
                            variant="secondary"
                            disabled={deleteSkillMutation.isPending}
                        >
                            Cancel
                        </Button>
                    </DialogClose>
                    <Button
                        type="submit"
                        variant="default"
                        className="flex-1 bg-red-500 text-white hover:bg-red-600"
                        onClick={() => submit(skillDeleteTargetId!)}
                        disabled={deleteSkillMutation.isPending}
                    >
                        Delete
                    </Button>
                </DialogFooter>
            </DialogContent>
        </Dialog>
    );
}
