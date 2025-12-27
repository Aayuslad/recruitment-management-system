'use client';

import * as React from 'react';

import { Button } from '@/components/ui/button';
import {
    Command,
    CommandGroup,
    CommandItem,
    CommandList,
} from '@/components/ui/command';
import {
    Popover,
    PopoverContent,
    PopoverTrigger,
} from '@/components/ui/popover';
import {
    INTERVIEW_PARTICIPANT_ROLE,
    type InterviewParticipantRole,
} from '@/types/enums';

const roles: { value: InterviewParticipantRole; label: string }[] = [
    {
        value: INTERVIEW_PARTICIPANT_ROLE.TECHNICAL_INTERVIEWER,
        label: 'Technical Interviewer',
    },
    {
        value: INTERVIEW_PARTICIPANT_ROLE.HR_INTERVIEWER,
        label: 'HR Interviewer',
    },
    {
        value: INTERVIEW_PARTICIPANT_ROLE.NOTE_TAKER,
        label: 'Note Taker',
    },
    {
        value: INTERVIEW_PARTICIPANT_ROLE.OBSERVER,
        label: 'Observer',
    },
];

type Props = {
    setSelectedRole: (role: InterviewParticipantRole) => void;
};

export function InterviewRoundRolesRequirementSelector({
    setSelectedRole,
}: Props) {
    const [open, setOpen] = React.useState(false);

    return (
        <Popover open={open} onOpenChange={setOpen}>
            <PopoverTrigger asChild>
                <Button variant="ghost" type="button" className="mx-auto">
                    + Add Role
                </Button>
            </PopoverTrigger>
            <PopoverContent className="w-[200px] p-0">
                <Command>
                    <CommandList>
                        <CommandGroup>
                            {roles.map((role) => (
                                <CommandItem
                                    key={role.value}
                                    value={role.value}
                                    onSelect={(currentValue) => {
                                        setSelectedRole(
                                            currentValue as InterviewParticipantRole
                                        );
                                        setOpen(false);
                                    }}
                                >
                                    {role.label}
                                </CommandItem>
                            ))}
                        </CommandGroup>
                    </CommandList>
                </Command>
            </PopoverContent>
        </Popover>
    );
}
