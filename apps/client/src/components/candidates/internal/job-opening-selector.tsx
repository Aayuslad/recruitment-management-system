'use client';

import { Check, ChevronsUpDown } from 'lucide-react';
import * as React from 'react';

import { useGetJobOpenings } from '@/api/job-opening-api';
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
import type { JobOpeningSummary } from '@/types/job-opening-types';

type Props = {
    setSelectedJobOpeningId: React.Dispatch<
        React.SetStateAction<string | undefined>
    >;
};

export function JobOpeningSelector({ setSelectedJobOpeningId }: Props) {
    const [open, setOpen] = React.useState(false);
    const [selectedJobOpening, setSelectedJobOpening] =
        React.useState<JobOpeningSummary>();
    const { data: jobOpenings, isLoading } = useGetJobOpenings();

    React.useEffect(() => {
        if (selectedJobOpening) {
            setSelectedJobOpeningId(selectedJobOpening.id);
        }
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [selectedJobOpening]);

    return (
        <Popover open={open} onOpenChange={setOpen}>
            <PopoverTrigger asChild>
                <Button
                    variant="outline"
                    role="combobox"
                    aria-expanded={open}
                    className="w-[350px] h-auto justify-between"
                >
                    {selectedJobOpening ? (
                        <div className="flex flex-col items-baseline">
                            <div>
                                <span>Title: </span>
                                <span className="font-bold">
                                    {selectedJobOpening.title}
                                </span>
                            </div>
                            <div className="space-x-2">
                                <span>
                                    {selectedJobOpening.designationName}
                                </span>
                                <span>•</span>
                                <span>{selectedJobOpening.jobLocation}</span>
                            </div>
                            <div className="space-x-2">
                                <span>
                                    ID{' '}
                                    <span className="text-muted-foreground">
                                        #{selectedJobOpening.id.slice(0, 6)}
                                    </span>
                                </span>
                                <span>•</span>
                                <span>
                                    Created by{' '}
                                    <span className="underline">
                                        {selectedJobOpening.createdByUserName}
                                    </span>
                                </span>
                            </div>
                        </div>
                    ) : (
                        'Select Job Opening...'
                    )}
                    <ChevronsUpDown className="opacity-50" />
                </Button>
            </PopoverTrigger>
            <PopoverContent className="w-[350px] p-0">
                <Command>
                    <CommandInput
                        placeholder="Search position batch..."
                        className="h-9"
                    />
                    <CommandList>
                        {!isLoading && !jobOpenings && (
                            <CommandEmpty>No job openings found.</CommandEmpty>
                        )}
                        {isLoading && (
                            <CommandEmpty>Loading job openings...</CommandEmpty>
                        )}
                        <CommandGroup>
                            {jobOpenings?.map((jobOpening) => (
                                <CommandItem
                                    value={jobOpening.id}
                                    data-keywords={jobOpening.designationName}
                                    onSelect={(val) => {
                                        const opening = jobOpenings!.find(
                                            (x) => x.id === val
                                        );
                                        setSelectedJobOpening(opening);
                                        setOpen(false);
                                    }}
                                >
                                    <div className="flex flex-col">
                                        <div>
                                            <span>Title: </span>
                                            <span className="font-bold">
                                                {jobOpening.title}
                                            </span>
                                        </div>
                                        <div className="space-x-2">
                                            <span>
                                                {jobOpening.designationName}
                                            </span>
                                            <span>•</span>
                                            <span>
                                                {jobOpening.jobLocation}
                                            </span>
                                        </div>
                                        <div className="space-x-2">
                                            <span>
                                                ID{' '}
                                                <span className="text-muted-foreground">
                                                    #{jobOpening.id.slice(0, 6)}
                                                </span>
                                            </span>
                                            <span>•</span>
                                            <span>
                                                Created by{' '}
                                                <span className="underline">
                                                    {
                                                        jobOpening.createdByUserName
                                                    }
                                                </span>
                                            </span>
                                        </div>
                                    </div>
                                    <Check
                                        className={cn(
                                            'ml-auto',
                                            selectedJobOpening?.id ===
                                                jobOpening.id
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
