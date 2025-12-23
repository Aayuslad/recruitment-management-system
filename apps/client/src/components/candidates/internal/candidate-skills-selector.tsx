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
import {
    Popover,
    PopoverContent,
    PopoverTrigger,
} from '@/components/ui/popover';
import { cn } from '@/lib/utils';
import type { CreateCandidateCommandCorrected } from '@/types/candidate-types';
import { Check, X } from 'lucide-react';
import React from 'react';
import { Label } from 'react-aria-components';

type Props = {
    fields: CreateCandidateCommandCorrected['skills'];
    append: (skill: CreateCandidateCommandCorrected['skills'][0]) => void;
    remove: (index: number) => void;
};

export function CandidateSkillsSelector({ fields, append, remove }: Props) {
    const [open, setOpen] = React.useState(false);
    const { data: skills, isLoading } = useGetSkills();

    if (isLoading) return <div>Loading skills...</div>;

    return (
        <div>
            <div className="flex justify-between">
                <Label>Skills</Label>

                <Popover open={open} onOpenChange={setOpen}>
                    <PopoverTrigger asChild>
                        <Button
                            variant="ghost"
                            role="combobox"
                            aria-expanded={open}
                            className="justify-between hover:cursor-pointer"
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
                                            key={skill.id}
                                            value={skill.name}
                                            onSelect={() => {
                                                const exists = fields?.some(
                                                    (s) =>
                                                        s.skillId === skill.id
                                                );

                                                if (exists) {
                                                    remove(
                                                        fields?.findIndex(
                                                            (s) =>
                                                                s.skillId ===
                                                                skill.id
                                                        )
                                                    );
                                                    return;
                                                }

                                                append({ skillId: skill.id });
                                            }}
                                        >
                                            {skill.name}
                                            <Check
                                                className={cn(
                                                    'ml-auto',
                                                    fields?.some(
                                                        (s) =>
                                                            s.skillId ===
                                                            skill.id
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

            <div className="py-2 px-2 border rounded-2xl">
                {fields.map((skill, index) => (
                    <Badge
                        key={index}
                        variant="outline"
                        className="text-sm font-normal pb-1 px-2.5 mr-1 mb-1"
                    >
                        {skills?.find((x) => x.id === skill.skillId)?.name}
                        <button
                            onClick={() => remove(index)}
                            className="ml-2 text-muted-foreground hover:text-foreground hover:cursor-pointer"
                        >
                            <X className="h-4 w-4" />
                        </button>
                    </Badge>
                ))}
                {fields.length === 0 && (
                    <div className="text-muted-foreground text-center">
                        No skills added.
                    </div>
                )}
            </div>
        </div>
    );
}
