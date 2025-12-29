'use client';

import { Check, ChevronsUpDown } from 'lucide-react';
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
import { cn } from '@/lib/utils';
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
    selectedRoles: InterviewParticipantRole[];
    addRole: (role: InterviewParticipantRole) => void;
    removeRole: (role: InterviewParticipantRole) => void;
};

export function InterviewParticipantRoleSelector({
    selectedRoles,
    addRole,
    removeRole,
}: Props) {
    const [open, setOpen] = React.useState(false);

    return (
        <Popover open={open} onOpenChange={setOpen}>
            <PopoverTrigger asChild>
                <Button
                    variant="outline"
                    role="combobox"
                    aria-expanded={open}
                    className="w-[170px] h-8 justify-between"
                >
                    <span>{selectedRoles.length}</span> Roles Selected
                    <ChevronsUpDown className="opacity-50" />
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
                                        const exist = selectedRoles.some(
                                            (selectedRole) =>
                                                selectedRole === role.value
                                        );

                                        if (exist) {
                                            removeRole(
                                                currentValue as InterviewParticipantRole
                                            );
                                        } else {
                                            addRole(
                                                currentValue as InterviewParticipantRole
                                            );
                                        }

                                        setOpen(false);
                                    }}
                                >
                                    {role.label}
                                    <Check
                                        className={cn(
                                            'ml-auto',
                                            selectedRoles.some(
                                                (selectedRole) =>
                                                    selectedRole === role.value
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
}
