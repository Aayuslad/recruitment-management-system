'use client';

import { Check, X } from 'lucide-react';
import * as React from 'react';

import { useGetUsersSummary } from '@/api/user-api';
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
import type { CreateJobOpeningCommandCorrected } from '@/types/job-opening-types';
import { Label } from '../../ui/label';
import { InterviewParticipantRoleSelector } from './interview-participant-role-selector';
import {
    INTERVIEW_PARTICIPANT_ROLE,
    type InterviewParticipantRole,
} from '@/types/enums';

type Props = {
    fealds: CreateJobOpeningCommandCorrected['interviewers'];
    append: (
        value: CreateJobOpeningCommandCorrected['interviewers'][0]
    ) => void;
    remove: (index: number) => void;
};

export function InterviewParticipantSelector({
    fealds,
    append,
    remove,
}: Props) {
    const [open, setOpen] = React.useState(false);
    const { data, isLoading } = useGetUsersSummary();

    return (
        <div>
            <div className="flex justify-between">
                <Label htmlFor="reviewers">Interview Participants</Label>
                <Popover open={open} onOpenChange={setOpen}>
                    <PopoverTrigger asChild>
                        <Button
                            variant="ghost"
                            role="combobox"
                            aria-expanded={open}
                        >
                            + Add Participant
                        </Button>
                    </PopoverTrigger>
                    <PopoverContent className="w-[400px] p-0">
                        <Command>
                            <CommandInput
                                placeholder="Search user names..."
                                className="h-9"
                            />
                            <CommandList>
                                <CommandEmpty>No Users found.</CommandEmpty>
                                {isLoading && (
                                    <CommandEmpty>
                                        Loading Users...
                                    </CommandEmpty>
                                )}
                                <CommandGroup>
                                    {data?.map((userSummary) => (
                                        <CommandItem
                                            key={userSummary.userId}
                                            value={userSummary.userId}
                                            disabled={fealds?.some(
                                                (x) =>
                                                    x.userId ===
                                                    userSummary.userId
                                            )}
                                            onSelect={(currentValue) => {
                                                append({
                                                    userId: currentValue,
                                                    role: INTERVIEW_PARTICIPANT_ROLE.TECHNICAL_INTERVIEWER,
                                                });
                                                setOpen(false);
                                            }}
                                        >
                                            {userSummary.userName}
                                            <span className="text-muted-foreground">
                                                ({userSummary.email})
                                            </span>
                                            <Check
                                                className={cn(
                                                    'ml-auto',
                                                    fealds?.some(
                                                        (x) =>
                                                            x.userId ===
                                                            userSummary.userId
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

            <div className="border rounded-2xl py-2 px-4 space-y-2.5">
                {fealds
                    .reduce(
                        (acc, feald) => {
                            if (!acc.find((x) => x.userId === feald.userId)) {
                                acc.push({
                                    userId: feald.userId,
                                    roles: [feald.role],
                                });
                            } else {
                                acc.find(
                                    (x) => x.userId === feald.userId
                                )?.roles.push(feald.role);
                            }

                            return acc;
                        },
                        [] as {
                            userId: string;
                            roles: InterviewParticipantRole[];
                        }[]
                    )
                    ?.map((participant) => (
                        <div
                            key={participant.userId}
                            className="flex items-center justify-between"
                        >
                            <span className="w-[300px] flex flex-col">
                                <span>
                                    {data?.find(
                                        (x) => x.userId === participant.userId
                                    )?.userName ?? '--'}
                                </span>
                                <span className="text-muted-foreground">
                                    (
                                    {data?.find(
                                        (x) => x.userId === participant.userId
                                    )?.email ?? '--'}
                                    )
                                </span>
                            </span>
                            <div className="flex gap-2">
                                <InterviewParticipantRoleSelector
                                    selectedRoles={participant.roles}
                                    addRole={(role) => {
                                        append({
                                            userId: participant.userId,
                                            role: role,
                                        });
                                    }}
                                    removeRole={(role) => {
                                        remove(
                                            fealds.findIndex(
                                                (x) =>
                                                    x.userId ===
                                                        participant.userId &&
                                                    x.role === role
                                            )
                                        );
                                    }}
                                />
                                <Button
                                    size="sm"
                                    variant="link"
                                    onClick={() =>
                                        fealds.map((x) => {
                                            if (
                                                x.userId === participant.userId
                                            ) {
                                                remove(fealds.indexOf(x));
                                            }
                                        })
                                    }
                                    className="hover:cursor-pointer"
                                >
                                    <X className="h-4 w-4" />
                                </Button>
                            </div>
                        </div>
                    ))}
                {fealds?.length === 0 && (
                    <div className="text-muted-foreground text-center py-3">
                        No Participants selected.
                    </div>
                )}
            </div>
        </div>
    );
}
