import { Badge } from '@/components/ui/badge';
import {
    Tooltip,
    TooltipContent,
    TooltipTrigger,
} from '@/components/ui/tooltip';
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
                                    <Tooltip key={x.skillId}>
                                        <TooltipTrigger asChild>
                                            <Badge
                                                variant="outline"
                                                className="text-sm font-normal pb-1.5 px-2.5 mr-1 mb-1"
                                            >
                                                <span>{x.name}</span>
                                                {x.minExperienceYears !== 0 && (
                                                    <span className="text-xs -mb-1 pb-[1px] px-1.5 bg-accent rounded-2xl">
                                                        {x.minExperienceYears}
                                                    </span>
                                                )}
                                            </Badge>
                                        </TooltipTrigger>
                                        <TooltipContent>
                                            <p>
                                                Minimum Experience years:{' '}
                                                {x.minExperienceYears}
                                            </p>
                                        </TooltipContent>
                                    </Tooltip>
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
                                    <Tooltip key={x.skillId}>
                                        <TooltipTrigger asChild>
                                            <Badge
                                                variant="outline"
                                                className="text-sm font-normal pb-1.5 px-2.5 mr-1 mb-1"
                                            >
                                                <span>{x.name}</span>
                                                {x.minExperienceYears !== 0 && (
                                                    <span className="text-xs -mb-1 pb-[1px] px-1.5 bg-accent rounded-2xl">
                                                        {x.minExperienceYears}
                                                    </span>
                                                )}
                                            </Badge>
                                        </TooltipTrigger>
                                        <TooltipContent>
                                            <p>
                                                Minimum Experience years:{' '}
                                                {x.minExperienceYears}
                                            </p>
                                        </TooltipContent>
                                    </Tooltip>
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
