import { SkillPill } from '@/components/ui/skill-pill';
import { useAppStore } from '@/store';
import type { Designation } from '@/types/designation-types';
import { useQueryClient } from '@tanstack/react-query';
import { useShallow } from 'zustand/react/shallow';

export const ViewDialogBody = () => {
    const queryClient = useQueryClient();
    const { designationDialogTarget } = useAppStore(
        useShallow((s) => ({
            designationDialogTarget: s.designationDialogTarget,
        }))
    );

    const designationViewTarget = queryClient
        .getQueryData<Designation[]>(['designations'])
        ?.find((x) => x.id === designationDialogTarget?.id);

    return (
        <div className="w-full space-y-6">
            <h2 className="font-bold text-2xl w-full text-center">
                {designationViewTarget?.name}
            </h2>
            <div className="space-y-4">
                <div className="space-y-3">
                    <h4>Required Skills</h4>
                    <div>
                        {designationViewTarget?.designationSkills
                            .filter((x) => x.skillType === 'Required')
                            .map((x) => {
                                return (
                                    <SkillPill id={x.skillId} name={x.name} />
                                );
                            })}
                        {designationViewTarget?.designationSkills.filter(
                            (x) => x.skillType === 'Required'
                        ).length === 0 && (
                            <div className="text-muted-foreground text-center text-sm">
                                No skills with type 'Required'
                            </div>
                        )}
                    </div>
                </div>
                <div className="space-y-3">
                    <h4>Preferred Skills</h4>
                    <div>
                        {designationViewTarget?.designationSkills
                            .filter((x) => x.skillType === 'Preferred')
                            .map((x) => {
                                return (
                                    <SkillPill id={x.skillId} name={x.name} />
                                );
                            })}
                        {designationViewTarget?.designationSkills.filter(
                            (x) => x.skillType === 'Preferred'
                        ).length === 0 && (
                            <div className="text-muted-foreground text-center text-sm">
                                No skills with type 'Preferred'
                            </div>
                        )}
                    </div>
                </div>
            </div>
        </div>
    );
};
