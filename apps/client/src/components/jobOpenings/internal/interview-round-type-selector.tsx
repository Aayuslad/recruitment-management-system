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
import { INTERVIEW_TYPE, type InterviewType } from '@/types/enums';

const types: { value: InterviewType; label: string }[] = [
    {
        value: INTERVIEW_TYPE.ONLINE_TEST,
        label: 'Online Test',
    },
    {
        value: INTERVIEW_TYPE.TECHNICAL,
        label: 'Technical',
    },
    {
        value: INTERVIEW_TYPE.HR,
        label: 'HR',
    },
];

type Props = {
    selectedType: InterviewType | undefined;
    setSelectedType: (type: InterviewType) => void;
};

export function InterviewRoundTypeSelector({
    selectedType,
    setSelectedType,
}: Props) {
    const [open, setOpen] = React.useState(false);

    return (
        <Popover open={open} onOpenChange={setOpen}>
            <PopoverTrigger asChild>
                <Button
                    variant="outline"
                    role="combobox"
                    aria-expanded={open}
                    className="w-[170px] h-7 justify-between"
                >
                    {selectedType
                        ? types.find((type) => type.value === selectedType)
                              ?.label
                        : 'Select Round Type...'}
                    <ChevronsUpDown className="opacity-50" />
                </Button>
            </PopoverTrigger>
            <PopoverContent className="w-[200px] p-0">
                <Command>
                    <CommandList>
                        <CommandGroup>
                            {types.map((type) => (
                                <CommandItem
                                    key={type.value}
                                    value={type.value}
                                    onSelect={(currentValue) => {
                                        setSelectedType(
                                            currentValue as InterviewType
                                        );
                                        setOpen(false);
                                    }}
                                >
                                    {type.label}
                                    <Check
                                        className={cn(
                                            'ml-auto',
                                            selectedType === type.value
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
