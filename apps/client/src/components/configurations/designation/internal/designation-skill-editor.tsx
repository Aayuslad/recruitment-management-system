import { useGetSkills } from '@/api/skill-api';
import { Button } from '@/components/ui/button';
import {
    Command,
    CommandEmpty,
    CommandGroup,
    CommandInput,
    CommandItem,
    CommandList,
} from '@/components/ui/command';
import { Input } from '@/components/ui/input';
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
import { cn } from '@/lib/utils';
import type { EditDesignationCommandCorrected } from '@/types/designation-types';
import type { SkillType } from '@/types/enums';
import type { SkillDetailDTO } from '@/types/job-opening-types';
import { Check, X } from 'lucide-react';
import * as React from 'react';

type Props = {
    fields: EditDesignationCommandCorrected['designationSkills'];
    append: (
        x: EditDesignationCommandCorrected['designationSkills'][0]
    ) => void;
    remove: (index: number) => void;
    update: (
        index: number,
        value: EditDesignationCommandCorrected['designationSkills'][0]
    ) => void;
};

export function DesignationSkillEditor({
    fields,
    append,
    remove,
    update,
}: Props) {
    const [open, setOpen] = React.useState(false);
    const [skills, setSkills] = React.useState<SkillDetailDTO[]>();
    const { data, isLoading } = useGetSkills();

    const handleSelect = (skill: SkillDetailDTO) => {
        append({
            skillId: skill.skillId,
            skillType: skill.skillType,
            minExperienceYears: skill.minExperienceYears ?? 0,
        });
        setOpen(false);
    };

    React.useEffect(() => {
        const skills =
            (data?.map((skill) => ({
                skillId: skill.id,
                skillName: skill.name,
                skillType: 'Required',
                minExperienceYears: 0,
            })) as SkillDetailDTO[]) ?? [];

        setSkills(skills);
    }, [data, setSkills, fields]);

    if (isLoading) return <div>Loading skills...</div>;

    return (
        <div className="flex flex-col">
            <div className="rounded-xl overflow-hidden">
                <div className="flex flex-col gap-2 max-h-[300px] overflow-y-auto border rounded-xl p-2 px-2.5">
                    {fields?.map((skill) => (
                        <div
                            key={skill.skillId}
                            className="flex items-center justify-between w-full gap-4 pr-1 -mr-2 py-1 pb-1.5"
                        >
                            <span className="text-left w-full max-w-[180px] text-wrap">
                                {
                                    skills?.find(
                                        (x) => x.skillId === skill.skillId
                                    )?.skillName
                                }
                            </span>

                            <div className="ml-auto">
                                <Select
                                    value={skill.skillType}
                                    onValueChange={(newValue: SkillType) =>
                                        update(fields.indexOf(skill), {
                                            ...skill,
                                            skillType: newValue,
                                        })
                                    }
                                >
                                    <SelectTrigger
                                        className="px-2 text-sm"
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
                                        <SelectItem value="NiceToHave">
                                            Nice To Have
                                        </SelectItem>
                                    </SelectContent>
                                </Select>
                            </div>

                            <div className="flex items-center gap-2">
                                <label
                                    htmlFor="minExperienceYears"
                                    className="text-sm text-muted-foreground"
                                >
                                    Exp:
                                </label>
                                <Input
                                    id="minExperienceYears"
                                    type="text"
                                    value={skill.minExperienceYears}
                                    onChange={(e) => {
                                        update(fields.indexOf(skill), {
                                            ...skill,
                                            minExperienceYears: Number(
                                                e.target.value
                                            ),
                                        });
                                    }}
                                    className="w-8 h-8 px-2 text-sm"
                                    placeholder="0"
                                    style={{ height: '28px' }}
                                    min={0}
                                    max={50}
                                />
                            </div>

                            <button
                                type="button"
                                onClick={() => remove(fields.indexOf(skill))}
                                className="ml-1 rounded-full hover:bg-muted hover:cursor-pointer"
                            >
                                <X className="h-4 w-4" />
                            </button>
                        </div>
                    ))}
                    {fields?.length === 0 && (
                        <div className="text-center text-muted-foreground py-10">
                            No skill selected
                        </div>
                    )}
                </div>
            </div>

            <Popover open={open} onOpenChange={setOpen}>
                <PopoverTrigger asChild>
                    <Button
                        variant="ghost"
                        role="combobox"
                        aria-expanded={open}
                        className="justify-between hover:cursor-pointer mx-auto mt-3"
                    >
                        + Add Skill
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
                                        key={skill.skillId}
                                        value={skill.skillName}
                                        onSelect={() => handleSelect(skill)}
                                    >
                                        {skill.skillName}
                                        <Check
                                            className={cn(
                                                'ml-auto',
                                                fields?.some(
                                                    (s) =>
                                                        s.skillId ===
                                                        skill.skillId
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
        </div>
    );
}
