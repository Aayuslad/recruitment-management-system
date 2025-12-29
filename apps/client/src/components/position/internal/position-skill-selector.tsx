/* eslint-disable indent */
import { useGetDesignation } from '@/api/designation-api';
import { useGetSkills } from '@/api/skill-api';
import { Badge } from '@/components/ui/badge';
import { Button } from '@/components/ui/button';
import {
    Command,
    CommandEmpty,
    CommandGroup,
    CommandInput,
    CommandItem,
    CommandList,
} from '@/components/ui/command';
import { Label } from '@/components/ui/label';
import {
    Popover,
    PopoverContent,
    PopoverTrigger,
} from '@/components/ui/popover';
import {
    Select,
    SelectContent,
    SelectItem,
    SelectTrigger,
    SelectValue,
} from '@/components/ui/select';
import { SkillPill } from '@/components/ui/skill-pill';
import {
    Tooltip,
    TooltipContent,
    TooltipTrigger,
} from '@/components/ui/tooltip';
import { cn } from '@/lib/utils';
import type { DesignationSkillDTO } from '@/types/designation-types';
import type { SkillType } from '@/types/enums';
import type { CreatePositionBatchCommandCorrected } from '@/types/position-types';
import type { Skill } from '@/types/skill-types';
import { Check, Info, Save, SquarePen, X } from 'lucide-react';
import { useEffect, useState } from 'react';
import { toast } from 'sonner';

type Props = {
    designationId: string;
    skillOverRides: CreatePositionBatchCommandCorrected['skillOverRides'];
    append: (
        value: CreatePositionBatchCommandCorrected['skillOverRides'][0]
    ) => void;
    remove: (index: number) => void;
    update: (
        index: number,
        value: CreatePositionBatchCommandCorrected['skillOverRides'][0]
    ) => void;
};

export const PositionSkillSelector = ({
    designationId,
    skillOverRides,
    append,
    remove,
    update,
}: Props) => {
    const { data: designation, isLoading: designationIsLoading } =
        useGetDesignation(designationId);
    const { data: skills, isLoading: skillIsLoading } = useGetSkills();
    const [viewMode, setViewMode] = useState<'view' | 'edit'>('view');
    const [finalSkills, setFinalSkills] = useState<DesignationSkillDTO[]>([]);
    const [inheritedSkills, setInheritedSkills] = useState<
        DesignationSkillDTO[]
    >([]);

    useEffect(() => {
        if (designation) {
            setFinalSkills(
                designation.designationSkills as DesignationSkillDTO[]
            );
            setInheritedSkills(
                designation.designationSkills as DesignationSkillDTO[]
            );
        }
    }, [designation]);

    // fun to add add skill over ride for "adding skill"
    const addNewSkill = (skill: Skill) => {
        const existInFinalSkills = finalSkills.some(
            (s) => s.skillId === skill.id
        );

        if (existInFinalSkills) {
            toast.error('Skill already exist');
        }

        append({
            skillId: skill.id,
            type: 'Required',
            actionType: 'Add',
        });

        setFinalSkills((prev) => [
            ...(prev ?? []),
            {
                skillId: skill.id,
                name: skill.name,
                skillType: 'Required',
                minExperienceYears: 0,
            },
        ]);
    };

    // fun to add add skill over ride for "removing skill"
    const removeInheritedSkill = (inheritedSkill: DesignationSkillDTO) => {
        append({
            id: null,
            skillId: inheritedSkill.skillId,
            comments: null,
            type: inheritedSkill.skillType,
            actionType: 'Remove',
        });

        setInheritedSkills((prev) => {
            return prev.filter((y) => y.skillId !== inheritedSkill.skillId);
        });

        setFinalSkills((prev) => {
            return prev.filter((y) => y.skillId !== inheritedSkill.skillId);
        });
    };

    // fun to add add skill over ride for "updating skill"
    const editInheritedSkill = (inheritedSkill: DesignationSkillDTO) => {
        append({
            id: null,
            skillId: inheritedSkill.skillId,
            comments: null,
            type: inheritedSkill.skillType,
            actionType: 'Update',
        });

        setInheritedSkills((prev) => {
            return prev.filter((y) => y.skillId !== inheritedSkill.skillId);
        });
    };

    const handleOverRideSkillTypeChange = (
        overRide: CreatePositionBatchCommandCorrected['skillOverRides'][0],
        newValue: SkillType
    ) => {
        update(skillOverRides.indexOf(overRide), {
            ...overRide,
            type: newValue,
        });

        setFinalSkills(
            (prev) =>
                prev?.map((s) =>
                    s.skillId === overRide.skillId
                        ? {
                              ...s,
                              skillType: newValue,
                          }
                        : s
                ) ?? []
        );
    };

    // fun to handle remove skill-override record
    const handleRemoveOverride = (
        overRide: CreatePositionBatchCommandCorrected['skillOverRides'][0]
    ) => {
        remove(skillOverRides.indexOf(overRide));
        setInheritedSkills((prev) => [
            ...(prev ?? []),
            designation?.designationSkills.find(
                (x) => x.skillId === overRide.skillId
            ) as DesignationSkillDTO,
        ]);

        if (overRide.actionType === 'Add') {
            setFinalSkills(
                (prev) =>
                    prev?.filter((s) => s.skillId !== overRide.skillId) ?? []
            );
        }

        if (overRide.actionType === 'Remove') {
            setFinalSkills((prev) => [
                ...(prev ?? []),
                {
                    skillId: overRide.skillId,
                    name:
                        skills?.find((x) => x.id === overRide.skillId)?.name ??
                        '',
                    skillType: overRide.type,
                },
            ]);
        }

        if (overRide.actionType === 'Update') {
            const relativeDesignationSkill =
                designation?.designationSkills.find(
                    (x) => x.skillId === overRide.skillId
                ) as DesignationSkillDTO;

            setFinalSkills((prev) =>
                prev.map((x) => {
                    if (x.skillId === overRide.skillId) {
                        return {
                            skillId: overRide.skillId,
                            name: relativeDesignationSkill?.name,
                            skillType: relativeDesignationSkill.skillType,
                        };
                    }

                    return x;
                })
            );
        }
    };

    return (
        <div className="">
            <div className="flex justify-between">
                <h3 className="font-semibold text-lg flex items-center gap-1">
                    <Label>Skills Criteria</Label>
                    <span>
                        <Tooltip>
                            <TooltipTrigger asChild>
                                <Info className="w-4 h-4" />
                            </TooltipTrigger>
                            <TooltipContent>
                                <p className="text-wrap max-w-[200px] font-semibold">
                                    Skills are sourced from the Designation you
                                    have selected. You can add remove, update
                                    skills from here.
                                </p>
                            </TooltipContent>
                        </Tooltip>
                    </span>
                </h3>
                <Button
                    title={viewMode === 'view' ? 'Edit' : 'Save'}
                    variant={'ghost'}
                    type="button"
                    disabled={!designationId}
                    onClick={() => {
                        setViewMode((prev) =>
                            prev === 'view' ? 'edit' : 'view'
                        );
                    }}
                >
                    {viewMode === 'view' ? (
                        <SquarePen />
                    ) : (
                        <Save className="text-green-600" />
                    )}
                </Button>
            </div>

            {viewMode === 'view' && designation && skills && (
                <div className="border rounded-2xl px-2 py-2">
                    {[
                        ...finalSkills.filter(
                            (x) => x.skillType === 'Required'
                        ),
                        ...finalSkills.filter(
                            (x) => x.skillType === 'Preferred'
                        ),
                    ].map((x) => {
                        return (
                            <SkillPill
                                id={x.skillId}
                                name={x.name}
                                type={x.skillType}
                            />
                        );
                    })}
                </div>
            )}

            {viewMode === 'edit' && designation && skills && (
                <div className="border rounded-2xl px-2 py-2 space-y-4">
                    <div className="space-y-2">
                        <h3>Inherited from Designation</h3>
                        <div>
                            {[
                                ...inheritedSkills.filter(
                                    (inheritedSkill) =>
                                        inheritedSkill.skillType === 'Required'
                                ),
                                ...inheritedSkills.filter(
                                    (inheritedSkill) =>
                                        inheritedSkill.skillType === 'Preferred'
                                ),
                            ].map((inheritedSkill) => {
                                return (
                                    <Tooltip key={inheritedSkill.skillId}>
                                        <TooltipTrigger asChild>
                                            <Badge
                                                variant="outline"
                                                className={`text-sm font-normal pb-1.5 px-2.5 mr-1 mb-1 ${inheritedSkill.skillType ? (inheritedSkill.skillType === 'Required' ? 'border-amber-950' : 'border-cyan-800') : ''}`}
                                            >
                                                <span>
                                                    {inheritedSkill.name}
                                                </span>
                                            </Badge>
                                        </TooltipTrigger>
                                        <TooltipContent>
                                            <p className="space-x-1.5 mb-1">
                                                <span className="font-semibold">
                                                    Type:
                                                </span>
                                                <span>
                                                    {inheritedSkill.skillType}
                                                </span>
                                            </p>
                                            <div className="space-x-2 mt-1">
                                                <button
                                                    type="button"
                                                    className="p-1 px-2 rounded-2xl border border-black hover:cursor-pointer"
                                                    onClick={() => {
                                                        editInheritedSkill(
                                                            inheritedSkill
                                                        );
                                                    }}
                                                >
                                                    Edit
                                                </button>
                                                <button
                                                    type="button"
                                                    className="p-1 px-2 rounded-2xl border border-black hover:cursor-pointer"
                                                    onClick={() => {
                                                        removeInheritedSkill(
                                                            inheritedSkill
                                                        );
                                                    }}
                                                >
                                                    Remove
                                                </button>
                                            </div>
                                        </TooltipContent>
                                    </Tooltip>
                                );
                            })}
                        </div>
                    </div>
                    <div>
                        <div className="flex justify-between">
                            <h3>Overrides</h3>
                            <AddSkillPopover
                                skills={skills}
                                skillOverRides={skillOverRides}
                                inheritedSkills={inheritedSkills}
                                addNewSkill={addNewSkill}
                            />
                        </div>
                        <div className="border rounded-2xl">
                            <div className="overflow-y-auto px-4 py-2">
                                {skillOverRides?.map((overRide) => (
                                    <div
                                        key={overRide.skillId}
                                        className="flex items-center justify-between w-full gap-4 pr-1 -mr-2 py-1 pb-1.5"
                                    >
                                        <span className="text-left w-full max-w-[220px] text-wrap">
                                            {
                                                skills?.find(
                                                    (x) =>
                                                        x.id ===
                                                        overRide.skillId
                                                )?.name
                                            }
                                        </span>

                                        <div className="ml-auto">
                                            <Select
                                                value={overRide.type}
                                                onValueChange={(
                                                    newValue: SkillType
                                                ) => {
                                                    handleOverRideSkillTypeChange(
                                                        overRide,
                                                        newValue
                                                    );
                                                }}
                                            >
                                                <SelectTrigger
                                                    className="px-2 text-sm"
                                                    disabled={
                                                        overRide.actionType ===
                                                        'Remove'
                                                    }
                                                    style={{ height: '28px' }}
                                                >
                                                    <SelectValue placeholder="Select Type" />
                                                </SelectTrigger>

                                                <SelectContent className="text-sm">
                                                    <SelectItem value="Required">
                                                        Required
                                                    </SelectItem>
                                                    <SelectItem value="Preferred">
                                                        Preferred
                                                    </SelectItem>
                                                </SelectContent>
                                            </Select>
                                        </div>

                                        <div className="text-muted-foreground text-sm">
                                            {overRide.actionType}ing
                                        </div>

                                        <button
                                            type="button"
                                            onClick={() => {
                                                handleRemoveOverride(overRide);
                                            }}
                                            className="ml-1 rounded-full hover:bg-muted hover:cursor-pointer"
                                        >
                                            <X className="h-4 w-4" />
                                        </button>
                                    </div>
                                ))}
                                {skillOverRides?.length === 0 && (
                                    <div className="text-center text-muted-foreground py-10">
                                        No skill overrides
                                    </div>
                                )}
                            </div>
                        </div>
                    </div>
                </div>
            )}

            {!designationId && (
                <div className="border rounded-2xl py-2 px-4">
                    <div className="text-muted-foreground text-center py-3">
                        Select Designation first
                    </div>

                    {(skillIsLoading || designationIsLoading) &&
                        designationId && (
                            <div className="text-muted-foreground text-center py-3">
                                Loading...
                            </div>
                        )}
                </div>
            )}
        </div>
    );
};

type AddSkillPopoverProps = {
    skills: Skill[];
    skillOverRides: CreatePositionBatchCommandCorrected['skillOverRides'];
    inheritedSkills: DesignationSkillDTO[];
    addNewSkill: (skill: Skill) => void;
};

const AddSkillPopover = ({
    skills,
    skillOverRides,
    inheritedSkills,
    addNewSkill,
}: AddSkillPopoverProps) => {
    return (
        <Popover>
            <PopoverTrigger asChild>
                <Button
                    variant="ghost"
                    type="button"
                    className="py-0 text-sm h-8"
                >
                    + Add New Skill
                </Button>
            </PopoverTrigger>
            <PopoverContent className="w-[200px] p-0">
                <Command>
                    <CommandInput
                        placeholder="Search skill..."
                        className="h-9"
                    />
                    <CommandList>
                        <CommandEmpty>No skill found.</CommandEmpty>
                        <CommandGroup className="overflow-y-auto">
                            {skills?.map((skill) => (
                                <CommandItem
                                    key={skill.id}
                                    value={skill.name}
                                    disabled={
                                        inheritedSkills?.some(
                                            (s) => s.skillId === skill.id
                                        ) ||
                                        skillOverRides.some(
                                            (s) => s.skillId === skill.id
                                        )
                                    }
                                    onSelect={() => {
                                        addNewSkill(skill);
                                    }}
                                >
                                    {skill.name}
                                    <Check
                                        className={cn(
                                            'ml-auto',
                                            inheritedSkills?.some(
                                                (s) => s.skillId === skill.id
                                            ) ||
                                                skillOverRides.some(
                                                    (s) =>
                                                        s.skillId === skill.id
                                                )
                                                ? 'opacity-100'
                                                : 'opacity-0'
                                        )}
                                    />
                                </CommandItem>
                            ))}
                        </CommandGroup>
                    </CommandList>
                </Command>
            </PopoverContent>
        </Popover>
    );
};
