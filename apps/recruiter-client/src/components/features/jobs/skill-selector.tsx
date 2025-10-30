'use client';

import { useGetSkills } from '@/api/skill';
import { Badge } from '@/components/ui/badge';
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
import { Check, X } from 'lucide-react';
import * as React from 'react';

type Props = {
    skills: { id: string; label: string }[];
    setSkills: React.Dispatch<
        React.SetStateAction<{ id: string; label: string }[]>
    >;
};

export function SkillSelector({ skills, setSkills }: Props) {
    const [open, setOpen] = React.useState(false);
    const [selectedSkills, setSelectedSkills] = React.useState<
        { id: string; label: string }[]
    >([]);
    const { data, isLoading } = useGetSkills();

    const handleSelect = (skill: { id: string; label: string }) => {
        if (!selectedSkills.some((s) => s.id === skill.id)) {
            setSelectedSkills((prev) => [...prev, skill]);
        }
        setOpen(false);
    };

    React.useEffect(() => {
        const skills =
            data?.map((skill) => ({
                id: skill.id,
                label: skill.name,
            })) ?? [];

        setSkills(skills);
    }, [data, setSkills]);

    const handleRemove = (value: string) => {
        setSelectedSkills((prev) => prev.filter((s) => s.id !== value));
    };

    if (isLoading) return <div>Loading skills...</div>;

    return (
        <div className="flex flex-col">
            {/* ComboBox */}
            <Popover open={open} onOpenChange={setOpen}>
                {/* Selected Badges */}
                <div className="flex flex-col gap-1 max-h-[300px] overflow-y-auto">
                    {selectedSkills.map((skill) => (
                        <div
                            key={skill.id}
                            // variant="secondary"
                            className="flex items-center justify-between w-full gap-2 px-2 pl-3 py-1 pb-1.5"
                        >
                            <span className="text-left">{skill.label}</span>

                            <Select>
                                <SelectTrigger className="w-[100px] h-8 px-2 text-sm">
                                    <SelectValue placeholder="Skill Type" />
                                </SelectTrigger>
                                <SelectContent className="text-sm">
                                    <SelectItem value="required">
                                        Required
                                    </SelectItem>
                                    <SelectItem value="preferred">
                                        Preferred
                                    </SelectItem>
                                    <SelectItem value="nicetohave">
                                        Nice To Have
                                    </SelectItem>
                                </SelectContent>
                            </Select>

                            <div className="flex items-center gap-1">
                                <label
                                    htmlFor="minExperienceYears"
                                    className="text-sm text-muted-foreground"
                                >
                                    Exp
                                </label>
                                <Input
                                    id="minExperienceYears"
                                    type="number"
                                    className="w-14 h-8 px-2 text-sm"
                                    placeholder="0"
                                    min={0}
                                />
                            </div>

                            <button
                                type="button"
                                onClick={() => handleRemove(skill.id)}
                                className="ml-1 rounded-full hover:bg-muted -mb-0.5 hover:cursor-pointer"
                            >
                                <X className="h-4 w-4" />
                            </button>
                        </div>
                    ))}
                </div>

                <PopoverTrigger asChild>
                    <Badge
                        variant="outline"
                        role="combobox"
                        aria-expanded={open}
                        className="justify-between hover:cursor-pointer text-sm px-3 pr-4 mx-auto"
                    >
                        + Add Skill
                    </Badge>
                </PopoverTrigger>

                <PopoverContent className="w-[200px] p-0">
                    <Command>
                        <CommandInput
                            placeholder="Search skill..."
                            className="h-9"
                        />
                        <CommandList>
                            <CommandEmpty>No skill found.</CommandEmpty>
                            <CommandGroup>
                                {skills.map((skill) => (
                                    <CommandItem
                                        key={skill.id}
                                        value={skill.id}
                                        onSelect={() => handleSelect(skill)}
                                    >
                                        {skill.label}
                                        <Check
                                            className={cn(
                                                'ml-auto',
                                                selectedSkills.some(
                                                    (s) => s.id === skill.id
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
