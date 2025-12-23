import { useGetCandidate } from '@/api/candidate-api';
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
import { Label } from '@/components/ui/label';
import {
    Popover,
    PopoverContent,
    PopoverTrigger,
} from '@/components/ui/popover';
import { cn } from '@/lib/utils';
import type { CreateJobApplicationFeedbackCommandCorrected } from '@/types/job-application-types';
import { Check, X } from 'lucide-react';
import { useState } from 'react';

type Props = {
    candidateId: string;
    skillFeedbacks: CreateJobApplicationFeedbackCommandCorrected['skillFeedbacks'];
    append: (
        skillFeedback: CreateJobApplicationFeedbackCommandCorrected['skillFeedbacks'][0]
    ) => void;
    remove: (index: number) => void;
    update: (
        index: number,
        skillFeedback: CreateJobApplicationFeedbackCommandCorrected['skillFeedbacks'][0]
    ) => void;
};

export const SkillFeedbackInput = ({
    candidateId,
    skillFeedbacks,
    append,
    remove,
    update,
}: Props) => {
    const [open, setOpen] = useState(false);
    const { data: candidate } = useGetCandidate(candidateId);

    return (
        <div>
            <div className="flex justify-between">
                <Label htmlFor="comment">Skill Feedbacks</Label>

                <Popover open={open} onOpenChange={setOpen}>
                    <PopoverTrigger asChild>
                        <Button variant="ghost" role="combobox" className="h-8">
                            + Select Candidate Skill
                        </Button>
                    </PopoverTrigger>
                    <PopoverContent className="w-[200px] p-0">
                        <Command>
                            <CommandInput
                                placeholder="Search framework..."
                                className="h-9"
                            />
                            <CommandList>
                                <CommandEmpty>No skills found.</CommandEmpty>
                                <CommandGroup>
                                    {candidate?.skills.map((skill) => (
                                        <CommandItem
                                            key={skill.skillId}
                                            value={skill.skillName}
                                            disabled={skillFeedbacks.some(
                                                (s) =>
                                                    s.skillId === skill.skillId
                                            )}
                                            onSelect={() => {
                                                append({
                                                    skillId: skill.skillId,
                                                    rating: 0,
                                                    assessedExpYears: 0,
                                                });
                                                setOpen(false);
                                            }}
                                        >
                                            {skill.skillName}
                                            <Check
                                                className={cn(
                                                    'ml-auto',
                                                    skillFeedbacks.some(
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

            <div className="rounded-xl overflow-hidden">
                <div className="flex flex-col gap-2 max-h-[300px] overflow-y-auto border rounded-xl p-2 px-2.5">
                    {skillFeedbacks?.map((skillFeedback) => (
                        <div
                            key={skillFeedback.skillId}
                            className="w-full space-y-1 border rounded-xl pt-1 pb-2 px-3"
                        >
                            <div className="flex items-center justify-between">
                                <span>
                                    {
                                        candidate?.skills.find(
                                            (x) =>
                                                x.skillId ===
                                                skillFeedback.skillId
                                        )?.skillName
                                    }
                                </span>

                                <button
                                    type="button"
                                    onClick={() =>
                                        remove(
                                            skillFeedbacks.indexOf(
                                                skillFeedback
                                            )
                                        )
                                    }
                                    className="rounded-full hover:bg-muted hover:cursor-pointer"
                                >
                                    <X className="h-4 w-4" />
                                </button>
                            </div>

                            <div className="w-full flex justify-around">
                                <div className="flex items-center gap-1.5">
                                    <Label>Rating:</Label>
                                    <Input
                                        type="number"
                                        min={0}
                                        max={10}
                                        className="w-[80px] h-7"
                                        value={skillFeedback.rating}
                                        onChange={(e) => {
                                            const rating = parseInt(
                                                e.target.value
                                            );
                                            update(
                                                skillFeedbacks.indexOf(
                                                    skillFeedback
                                                ),
                                                {
                                                    ...skillFeedback,
                                                    rating,
                                                }
                                            );
                                        }}
                                    />
                                </div>

                                <div className="flex items-center gap-1.5">
                                    <Label>Assesed Experience:</Label>
                                    <Input
                                        type="number"
                                        step={1}
                                        className="w-[80px] h-7"
                                        value={
                                            skillFeedback.assessedExpYears ?? 0
                                        }
                                        onChange={(e) => {
                                            const assessedExpYears = parseInt(
                                                e.target.value
                                            );
                                            update(
                                                skillFeedbacks.indexOf(
                                                    skillFeedback
                                                ),
                                                {
                                                    ...skillFeedback,
                                                    assessedExpYears,
                                                }
                                            );
                                        }}
                                    />
                                </div>
                            </div>
                        </div>
                    ))}
                    {skillFeedbacks?.length === 0 && (
                        <div className="text-center text-muted-foreground py-10">
                            No skill feedbacks
                        </div>
                    )}
                </div>
            </div>
        </div>
    );
};
