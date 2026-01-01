'use client';

import { Check, Info, X } from 'lucide-react';
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
import {
    Tooltip,
    TooltipContent,
    TooltipTrigger,
} from '@/components/ui/tooltip';
import { cn } from '@/lib/utils';
import { INTERVIEW_PARTICIPANT_ROLE } from '@/types/enums';
import type { CreateJobOpeningCommandCorrected } from '@/types/job-opening-types';
import { Label } from '../../ui/label';

type Props = {
    fields?: CreateJobOpeningCommandCorrected['interviewers'];
    append: (
        value: CreateJobOpeningCommandCorrected['interviewers'][0]
    ) => void;
    remove: (index: number) => void;
    interviewRounds?: CreateJobOpeningCommandCorrected['interviewRounds'];
};

export type ParticipantPoolRequirements = {
    technicalInterviewers: number;
    hrInterviewers: number;
    observers: number;
    noteTackers: number;
};

export function InterviewParticipantSelector({
    fields,
    append,
    remove,
    interviewRounds,
}: Props) {
    const [
        technicalInterviewersSelectOpen,
        setTechnicalInterviewersSelectOpen,
    ] = React.useState(false);
    const [hrInterviewersSelectOpen, setHrInterviewersSelectOpen] =
        React.useState(false);
    const [observersSelectOpen, setObserversSelectOpen] = React.useState(false);
    const [noteTackersSelectOpen, setNoteTackersSelectOpen] =
        React.useState(false);
    const { data: users, isLoading } = useGetUsersSummary();
    const [participantPoolRequirements, setParticipantPoolRequirements] =
        React.useState<ParticipantPoolRequirements>({
            technicalInterviewers: 0,
            hrInterviewers: 0,
            observers: 0,
            noteTackers: 0,
        });

    React.useEffect(() => {
        setParticipantPoolRequirements({
            technicalInterviewers: 0,
            hrInterviewers: 0,
            observers: 0,
            noteTackers: 0,
        });

        interviewRounds?.forEach((round) => {
            round.requirements.forEach((requirement) => {
                if (requirement.role === 'TechnicalInterviewer')
                    setParticipantPoolRequirements((prev) => ({
                        ...prev,
                        technicalInterviewers: Math.max(
                            prev.technicalInterviewers,
                            requirement.requirementCount
                        ),
                    }));
                if (requirement.role === 'HRInterviewer')
                    setParticipantPoolRequirements((prev) => ({
                        ...prev,
                        hrInterviewers: Math.max(
                            prev.hrInterviewers,
                            requirement.requirementCount
                        ),
                    }));
                if (requirement.role === 'Observer')
                    setParticipantPoolRequirements((prev) => ({
                        ...prev,
                        observers: Math.max(
                            prev.observers,
                            requirement.requirementCount
                        ),
                    }));
                if (requirement.role === 'NoteTaker')
                    setParticipantPoolRequirements((prev) => ({
                        ...prev,
                        noteTackers: Math.max(
                            prev.noteTackers,
                            requirement.requirementCount
                        ),
                    }));
            });
        });
    }, [interviewRounds]);

    return (
        <div className="">
            <div className="flex justify-between mb-2">
                <h3 className="font-semibold text-lg flex items-center gap-1">
                    <Label>Participant Pool:</Label>
                    <span>
                        <Tooltip>
                            <TooltipTrigger asChild>
                                <Info className="w-4 h-4" />
                            </TooltipTrigger>
                            <TooltipContent>
                                <p className="text-wrap max-w-[200px] font-semibold">
                                    These participants will be assigned randomly
                                    to the interviews defined above, based on
                                    their role and panel requirements.
                                </p>
                            </TooltipContent>
                        </Tooltip>
                    </span>
                </h3>
            </div>

            <div className="border rounded-2xl py-3">
                {participantPoolRequirements.technicalInterviewers === 0 &&
                    participantPoolRequirements.hrInterviewers === 0 &&
                    participantPoolRequirements.observers === 0 &&
                    participantPoolRequirements.noteTackers === 0 &&
                    fields?.length === 0 && (
                        <div>
                            <p className="text-center py-4 text-muted-foreground">
                                No participants required yet
                            </p>
                        </div>
                    )}

                {(participantPoolRequirements?.technicalInterviewers > 0 ||
                    fields?.some(
                        (field) =>
                            field.role ===
                            INTERVIEW_PARTICIPANT_ROLE.TECHNICAL_INTERVIEWER
                    )) && (
                    <div className="py-2 px-4">
                        <div className="flex justify-between items-center border-b">
                            <div className="flex gap-2 items-center">
                                <h4 className="text-sm font-semibold">
                                    Technical Interviewers
                                </h4>
                                <span className="text-muted-foreground">
                                    (Minimum{' '}
                                    {
                                        participantPoolRequirements?.technicalInterviewers
                                    }{' '}
                                    required)
                                </span>
                            </div>

                            <Popover
                                open={technicalInterviewersSelectOpen}
                                onOpenChange={
                                    setTechnicalInterviewersSelectOpen
                                }
                            >
                                <PopoverTrigger asChild>
                                    <Button
                                        variant="ghost"
                                        type="button"
                                        role="combobox"
                                        aria-expanded={
                                            technicalInterviewersSelectOpen
                                        }
                                    >
                                        + Add
                                    </Button>
                                </PopoverTrigger>
                                <PopoverContent className="w-[400px] p-0">
                                    <Command>
                                        <CommandInput
                                            placeholder="Search user names..."
                                            className="h-9"
                                        />
                                        <CommandList>
                                            <CommandEmpty>
                                                No Users found.
                                            </CommandEmpty>
                                            {isLoading && (
                                                <CommandEmpty>
                                                    Loading Users...
                                                </CommandEmpty>
                                            )}
                                            <CommandGroup>
                                                {users?.map((userSummary) => (
                                                    <CommandItem
                                                        key={userSummary.userId}
                                                        value={
                                                            userSummary.userId
                                                        }
                                                        disabled={fields?.some(
                                                            (x) =>
                                                                x.userId ===
                                                                userSummary.userId
                                                        )}
                                                        onSelect={(
                                                            currentValue
                                                        ) => {
                                                            append({
                                                                userId: currentValue,
                                                                role: INTERVIEW_PARTICIPANT_ROLE.TECHNICAL_INTERVIEWER,
                                                            });
                                                            setTechnicalInterviewersSelectOpen(
                                                                false
                                                            );
                                                        }}
                                                    >
                                                        <div>
                                                            <div className="text-sm space-x-1.5">
                                                                <span>
                                                                    {
                                                                        userSummary.userName
                                                                    }
                                                                </span>
                                                                <span className="text-muted-foreground">
                                                                    {userSummary.roles.map(
                                                                        (
                                                                            role
                                                                        ) => (
                                                                            <span className="border text-xs pb-0.5 px-1.5 rounded-lg">
                                                                                {
                                                                                    role.name
                                                                                }
                                                                            </span>
                                                                        )
                                                                    )}
                                                                </span>
                                                            </div>
                                                        </div>
                                                        <Check
                                                            className={cn(
                                                                'ml-auto',
                                                                fields?.some(
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

                        <ul className="px-4 space-y-0.5 list-disc">
                            {fields?.filter(
                                (x) =>
                                    x.role ===
                                    INTERVIEW_PARTICIPANT_ROLE.TECHNICAL_INTERVIEWER
                            ).length === 0 && (
                                <div className="text-muted-foreground w-full text-sm pt-3 pb-3 text-center">
                                    No Technical Interviewers Selected
                                </div>
                            )}

                            {fields
                                ?.filter(
                                    (x) =>
                                        x.role ===
                                        INTERVIEW_PARTICIPANT_ROLE.TECHNICAL_INTERVIEWER
                                )
                                .map((participant) => (
                                    <li className="flex items-center justify-between">
                                        <span className="text-sm flex items-center gap-1.5">
                                            <span>
                                                {
                                                    users?.find(
                                                        (x) =>
                                                            x.userId ===
                                                            participant.userId
                                                    )?.userName
                                                }
                                            </span>
                                            <span className="text-muted-foreground">
                                                (
                                                {
                                                    users?.find(
                                                        (x) =>
                                                            x.userId ===
                                                            participant.userId
                                                    )?.email
                                                }
                                                )
                                            </span>
                                            <span>
                                                {users
                                                    ?.find(
                                                        (x) =>
                                                            x.userId ===
                                                            participant.userId
                                                    )
                                                    ?.roles.map((role) => (
                                                        <span className="border text-xs pb-0.5 px-1.5 rounded-lg">
                                                            {role.name}
                                                        </span>
                                                    ))}
                                            </span>
                                        </span>
                                        <div className="flex gap-2">
                                            <Button
                                                type="button"
                                                size="sm"
                                                variant="link"
                                                onClick={() =>
                                                    remove(
                                                        fields.indexOf(
                                                            participant
                                                        )
                                                    )
                                                }
                                                className="hover:cursor-pointer"
                                            >
                                                <X className="h-4 w-4" />
                                            </Button>
                                        </div>
                                    </li>
                                ))}
                        </ul>
                    </div>
                )}

                {(participantPoolRequirements?.hrInterviewers > 0 ||
                    fields?.some(
                        (field) =>
                            field.role ===
                            INTERVIEW_PARTICIPANT_ROLE.HR_INTERVIEWER
                    )) && (
                    <div className=" py-2 px-4 space-y">
                        <div className="flex justify-between items-center border-b">
                            <div className="flex gap-2 items-center">
                                <h4 className="text-sm font-semibold">
                                    HR Interviewers
                                </h4>
                                <span className="text-muted-foreground">
                                    (Minimum{' '}
                                    {
                                        participantPoolRequirements?.hrInterviewers
                                    }{' '}
                                    required)
                                </span>
                            </div>

                            <Popover
                                open={hrInterviewersSelectOpen}
                                onOpenChange={setHrInterviewersSelectOpen}
                            >
                                <PopoverTrigger asChild>
                                    <Button
                                        variant="ghost"
                                        role="combobox"
                                        type="button"
                                        aria-expanded={hrInterviewersSelectOpen}
                                    >
                                        + Add
                                    </Button>
                                </PopoverTrigger>
                                <PopoverContent className="w-[400px] p-0">
                                    <Command>
                                        <CommandInput
                                            placeholder="Search user names..."
                                            className="h-9"
                                        />
                                        <CommandList>
                                            <CommandEmpty>
                                                No Users found.
                                            </CommandEmpty>
                                            {isLoading && (
                                                <CommandEmpty>
                                                    Loading Users...
                                                </CommandEmpty>
                                            )}
                                            <CommandGroup>
                                                {users?.map((userSummary) => (
                                                    <CommandItem
                                                        key={userSummary.userId}
                                                        value={
                                                            userSummary.userId
                                                        }
                                                        disabled={fields?.some(
                                                            (x) =>
                                                                x.userId ===
                                                                userSummary.userId
                                                        )}
                                                        onSelect={(
                                                            currentValue
                                                        ) => {
                                                            append({
                                                                userId: currentValue,
                                                                role: INTERVIEW_PARTICIPANT_ROLE.HR_INTERVIEWER,
                                                            });
                                                            setHrInterviewersSelectOpen(
                                                                false
                                                            );
                                                        }}
                                                    >
                                                        <div>
                                                            <div className="text-sm space-x-1.5">
                                                                <span>
                                                                    {
                                                                        userSummary.userName
                                                                    }
                                                                </span>
                                                                <span className="text-muted-foreground">
                                                                    {userSummary.roles.map(
                                                                        (
                                                                            role
                                                                        ) => (
                                                                            <span className="border text-xs pb-0.5 px-1.5 rounded-lg">
                                                                                {
                                                                                    role.name
                                                                                }
                                                                            </span>
                                                                        )
                                                                    )}
                                                                </span>
                                                            </div>
                                                        </div>
                                                        <Check
                                                            className={cn(
                                                                'ml-auto',
                                                                fields?.some(
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

                        <ul className="px-4 space-y-0.5 list-disc">
                            {fields?.filter(
                                (x) =>
                                    x.role ===
                                    INTERVIEW_PARTICIPANT_ROLE.HR_INTERVIEWER
                            ).length === 0 && (
                                <div className="text-muted-foreground w-full text-sm pt-3 pb-3 text-center">
                                    No HR Interviewers Selected
                                </div>
                            )}

                            {fields
                                ?.filter(
                                    (x) =>
                                        x.role ===
                                        INTERVIEW_PARTICIPANT_ROLE.HR_INTERVIEWER
                                )
                                .map((participant) => (
                                    <li className="flex items-center justify-between">
                                        <span className="text-sm flex items-center gap-1.5">
                                            <span>
                                                {
                                                    users?.find(
                                                        (x) =>
                                                            x.userId ===
                                                            participant.userId
                                                    )?.userName
                                                }
                                            </span>
                                            <span className="text-muted-foreground">
                                                (
                                                {
                                                    users?.find(
                                                        (x) =>
                                                            x.userId ===
                                                            participant.userId
                                                    )?.email
                                                }
                                                )
                                            </span>
                                            <span>
                                                {users
                                                    ?.find(
                                                        (x) =>
                                                            x.userId ===
                                                            participant.userId
                                                    )
                                                    ?.roles.map((role) => (
                                                        <span className="border text-xs pb-0.5 px-1.5 rounded-lg">
                                                            {role.name}
                                                        </span>
                                                    ))}
                                            </span>
                                        </span>
                                        <div className="flex gap-2">
                                            <Button
                                                type="button"
                                                size="sm"
                                                variant="link"
                                                onClick={() =>
                                                    remove(
                                                        fields.indexOf(
                                                            participant
                                                        )
                                                    )
                                                }
                                                className="hover:cursor-pointer"
                                            >
                                                <X className="h-4 w-4" />
                                            </Button>
                                        </div>
                                    </li>
                                ))}
                        </ul>
                    </div>
                )}

                {(participantPoolRequirements?.observers > 0 ||
                    fields?.some(
                        (field) =>
                            field.role === INTERVIEW_PARTICIPANT_ROLE.OBSERVER
                    )) && (
                    <div className=" py-2 px-4 space-y">
                        <div className="flex justify-between items-center border-b">
                            <div className="flex gap-2 items-center">
                                <h4 className="text-sm font-semibold">
                                    Observers
                                </h4>
                                <span className="text-muted-foreground">
                                    (Minimum{' '}
                                    {participantPoolRequirements?.observers}{' '}
                                    required)
                                </span>
                            </div>
                            <Popover
                                open={observersSelectOpen}
                                onOpenChange={setObserversSelectOpen}
                            >
                                <PopoverTrigger asChild>
                                    <Button
                                        variant="ghost"
                                        role="combobox"
                                        type="button"
                                        aria-expanded={observersSelectOpen}
                                    >
                                        + Add
                                    </Button>
                                </PopoverTrigger>
                                <PopoverContent className="w-[400px] p-0">
                                    <Command>
                                        <CommandInput
                                            placeholder="Search user names..."
                                            className="h-9"
                                        />
                                        <CommandList>
                                            <CommandEmpty>
                                                No Users found.
                                            </CommandEmpty>
                                            {isLoading && (
                                                <CommandEmpty>
                                                    Loading Users...
                                                </CommandEmpty>
                                            )}
                                            <CommandGroup>
                                                {users?.map((userSummary) => (
                                                    <CommandItem
                                                        key={userSummary.userId}
                                                        value={
                                                            userSummary.userId
                                                        }
                                                        disabled={fields?.some(
                                                            (x) =>
                                                                x.userId ===
                                                                userSummary.userId
                                                        )}
                                                        onSelect={(
                                                            currentValue
                                                        ) => {
                                                            append({
                                                                userId: currentValue,
                                                                role: INTERVIEW_PARTICIPANT_ROLE.OBSERVER,
                                                            });
                                                            setObserversSelectOpen(
                                                                false
                                                            );
                                                        }}
                                                    >
                                                        <div>
                                                            <div className="text-sm space-x-1.5">
                                                                <span>
                                                                    {
                                                                        userSummary.userName
                                                                    }
                                                                </span>
                                                                <span className="text-muted-foreground">
                                                                    {userSummary.roles.map(
                                                                        (
                                                                            role
                                                                        ) => (
                                                                            <span className="border text-xs pb-0.5 px-1.5 rounded-lg">
                                                                                {
                                                                                    role.name
                                                                                }
                                                                            </span>
                                                                        )
                                                                    )}
                                                                </span>
                                                            </div>
                                                        </div>
                                                        <Check
                                                            className={cn(
                                                                'ml-auto',
                                                                fields?.some(
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

                        <ul className="px-4 space-y-0.5 list-disc">
                            {fields?.filter(
                                (x) =>
                                    x.role ===
                                    INTERVIEW_PARTICIPANT_ROLE.OBSERVER
                            ).length === 0 && (
                                <div className="text-muted-foreground w-full text-sm pt-3 pb-3 text-center">
                                    No Observers Selected
                                </div>
                            )}

                            {fields
                                ?.filter(
                                    (x) =>
                                        x.role ===
                                        INTERVIEW_PARTICIPANT_ROLE.OBSERVER
                                )
                                .map((participant) => (
                                    <li className="flex items-center justify-between">
                                        <span className="text-sm flex items-center gap-1.5">
                                            <span>
                                                {
                                                    users?.find(
                                                        (x) =>
                                                            x.userId ===
                                                            participant.userId
                                                    )?.userName
                                                }
                                            </span>
                                            <span className="text-muted-foreground">
                                                (
                                                {
                                                    users?.find(
                                                        (x) =>
                                                            x.userId ===
                                                            participant.userId
                                                    )?.email
                                                }
                                                )
                                            </span>
                                            <span>
                                                {users
                                                    ?.find(
                                                        (x) =>
                                                            x.userId ===
                                                            participant.userId
                                                    )
                                                    ?.roles.map((role) => (
                                                        <span className="border text-xs pb-0.5 px-1.5 rounded-lg">
                                                            {role.name}
                                                        </span>
                                                    ))}
                                            </span>
                                        </span>
                                        <div className="flex gap-2">
                                            <Button
                                                type="button"
                                                size="sm"
                                                variant="link"
                                                onClick={() =>
                                                    remove(
                                                        fields.indexOf(
                                                            participant
                                                        )
                                                    )
                                                }
                                                className="hover:cursor-pointer"
                                            >
                                                <X className="h-4 w-4" />
                                            </Button>
                                        </div>
                                    </li>
                                ))}
                        </ul>
                    </div>
                )}

                {(participantPoolRequirements?.noteTackers > 0 ||
                    fields?.some(
                        (field) =>
                            field.role === INTERVIEW_PARTICIPANT_ROLE.NOTE_TAKER
                    )) && (
                    <div className=" py-2 px-4 space-y">
                        <div className="flex justify-between items-center border-b">
                            <div className="flex gap-2 items-center">
                                <h4 className="text-sm font-semibold">
                                    Note Takers
                                </h4>
                                <span className="text-muted-foreground">
                                    (Minimum{' '}
                                    {participantPoolRequirements?.noteTackers}{' '}
                                    required)
                                </span>
                            </div>

                            <Popover
                                open={noteTackersSelectOpen}
                                onOpenChange={setNoteTackersSelectOpen}
                            >
                                <PopoverTrigger asChild>
                                    <Button
                                        variant="ghost"
                                        role="combobox"
                                        type="button"
                                        aria-expanded={noteTackersSelectOpen}
                                    >
                                        + Add
                                    </Button>
                                </PopoverTrigger>
                                <PopoverContent className="w-[400px] p-0">
                                    <Command>
                                        <CommandInput
                                            placeholder="Search user names..."
                                            className="h-9"
                                        />
                                        <CommandList>
                                            <CommandEmpty>
                                                No Users found.
                                            </CommandEmpty>
                                            {isLoading && (
                                                <CommandEmpty>
                                                    Loading Users...
                                                </CommandEmpty>
                                            )}
                                            <CommandGroup>
                                                {users?.map((userSummary) => (
                                                    <CommandItem
                                                        key={userSummary.userId}
                                                        value={
                                                            userSummary.userId
                                                        }
                                                        disabled={fields?.some(
                                                            (x) =>
                                                                x.userId ===
                                                                userSummary.userId
                                                        )}
                                                        onSelect={(
                                                            currentValue
                                                        ) => {
                                                            append({
                                                                userId: currentValue,
                                                                role: INTERVIEW_PARTICIPANT_ROLE.NOTE_TAKER,
                                                            });
                                                            setNoteTackersSelectOpen(
                                                                false
                                                            );
                                                        }}
                                                    >
                                                        <div>
                                                            <div className="text-sm space-x-1.5">
                                                                <span>
                                                                    {
                                                                        userSummary.userName
                                                                    }
                                                                </span>
                                                                <span className="text-muted-foreground">
                                                                    {userSummary.roles.map(
                                                                        (
                                                                            role
                                                                        ) => (
                                                                            <span className="border text-xs pb-0.5 px-1.5 rounded-lg">
                                                                                {
                                                                                    role.name
                                                                                }
                                                                            </span>
                                                                        )
                                                                    )}
                                                                </span>
                                                            </div>
                                                        </div>
                                                        <Check
                                                            className={cn(
                                                                'ml-auto',
                                                                fields?.some(
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

                        <ul className="px-4 space-y-0.5 list-disc">
                            {fields?.filter(
                                (x) =>
                                    x.role ===
                                    INTERVIEW_PARTICIPANT_ROLE.NOTE_TAKER
                            ).length === 0 && (
                                <div className="text-muted-foreground w-full text-sm pt-3 pb-3 text-center">
                                    No Note Takers Selected
                                </div>
                            )}

                            {fields
                                ?.filter(
                                    (x) =>
                                        x.role ===
                                        INTERVIEW_PARTICIPANT_ROLE.NOTE_TAKER
                                )
                                .map((participant) => (
                                    <li className="flex items-center justify-between">
                                        <span className="text-sm flex items-center gap-1.5">
                                            <span>
                                                {
                                                    users?.find(
                                                        (x) =>
                                                            x.userId ===
                                                            participant.userId
                                                    )?.userName
                                                }
                                            </span>
                                            <span className="text-muted-foreground">
                                                (
                                                {
                                                    users?.find(
                                                        (x) =>
                                                            x.userId ===
                                                            participant.userId
                                                    )?.email
                                                }
                                                )
                                            </span>
                                            <span>
                                                {users
                                                    ?.find(
                                                        (x) =>
                                                            x.userId ===
                                                            participant.userId
                                                    )
                                                    ?.roles.map((role) => (
                                                        <span className="border text-xs pb-0.5 px-1.5 rounded-lg">
                                                            {role.name}
                                                        </span>
                                                    ))}
                                            </span>
                                        </span>
                                        <div className="flex gap-2">
                                            <Button
                                                type="button"
                                                size="sm"
                                                variant="link"
                                                onClick={() =>
                                                    remove(
                                                        fields.indexOf(
                                                            participant
                                                        )
                                                    )
                                                }
                                                className="hover:cursor-pointer"
                                            >
                                                <X className="h-4 w-4" />
                                            </Button>
                                        </div>
                                    </li>
                                ))}
                        </ul>
                    </div>
                )}
            </div>
        </div>
    );
}
