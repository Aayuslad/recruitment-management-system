'use client';

import { Check, ChevronsUpDown } from 'lucide-react';
import * as React from 'react';

import { useGetPositionBatches } from '@/api/position-api';
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
import type { PositionBatchSummary } from '@/types/position-types';

type Props = {
    setSelectedPositionBatchId: (id: string) => void;
};

export function PositionBatchSelector({ setSelectedPositionBatchId }: Props) {
    const [open, setOpen] = React.useState(false);
    const [selectedPositionBatch, setSelectedPositionBatch] =
        React.useState<PositionBatchSummary>();
    const { data, isLoading } = useGetPositionBatches();

    React.useEffect(() => {
        if (selectedPositionBatch) {
            setSelectedPositionBatchId(selectedPositionBatch.batchId);
        }
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [selectedPositionBatch]);

    return (
        <Popover open={open} onOpenChange={setOpen}>
            <PopoverTrigger asChild>
                <Button
                    variant="outline"
                    role="combobox"
                    aria-expanded={open}
                    className="w-[350px] h-auto justify-between"
                >
                    {selectedPositionBatch ? (
                        <div className="flex flex-col items-baseline">
                            <div className="space-x-2">
                                <span>
                                    {selectedPositionBatch.designationName}
                                </span>
                                <span>•</span>
                                <span>{selectedPositionBatch.jobLocation}</span>
                            </div>
                            <div className="space-x-2">
                                <span>
                                    {selectedPositionBatch.minCTC} -{' '}
                                    {selectedPositionBatch.maxCTC} LPA
                                </span>
                                <span>•</span>

                                <span>
                                    {selectedPositionBatch.positionsCount}{' '}
                                    Positions
                                </span>
                            </div>
                            <div className="space-x-2">
                                <span>
                                    Batch{' '}
                                    <span className="text-muted-foreground">
                                        #
                                        {selectedPositionBatch.batchId.slice(
                                            0,
                                            6
                                        )}
                                    </span>
                                </span>
                                <span>•</span>
                                <span>
                                    Created by{' '}
                                    <span className="underline">
                                        {
                                            selectedPositionBatch.createdByUserName
                                        }
                                    </span>
                                </span>
                            </div>
                        </div>
                    ) : (
                        'Select Position Batch...'
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
                        {!isLoading && !data && (
                            <CommandEmpty>
                                No position batch found.
                            </CommandEmpty>
                        )}
                        {isLoading && (
                            <CommandEmpty>
                                Loading position bacthes...
                            </CommandEmpty>
                        )}
                        <CommandGroup>
                            {data?.map((positionBatch) => (
                                <CommandItem
                                    value={positionBatch.batchId}
                                    data-keywords={
                                        positionBatch.designationName
                                    }
                                    onSelect={(val) => {
                                        const batch = data!.find(
                                            (x) => x.batchId === val
                                        );
                                        setSelectedPositionBatch(batch);
                                        setOpen(false);
                                    }}
                                >
                                    <div className="flex flex-col">
                                        <div className="space-x-2">
                                            <span>
                                                {positionBatch.designationName}
                                            </span>
                                            <span>•</span>
                                            <span>
                                                {positionBatch.jobLocation}
                                            </span>
                                        </div>
                                        <div className="space-x-2">
                                            <span>
                                                {positionBatch.minCTC} -{' '}
                                                {positionBatch.maxCTC} LPA
                                            </span>
                                            <span>•</span>
                                            <span>
                                                {positionBatch.positionsCount}{' '}
                                                Positions
                                            </span>
                                        </div>
                                        <div className="space-x-2">
                                            <span>
                                                Batch{' '}
                                                <span className="text-muted-foreground">
                                                    #
                                                    {positionBatch.batchId.slice(
                                                        0,
                                                        6
                                                    )}
                                                </span>
                                            </span>
                                            <span>•</span>
                                            <span>
                                                Created by{' '}
                                                <span className="underline">
                                                    {
                                                        positionBatch.createdByUserName
                                                    }
                                                </span>
                                            </span>
                                        </div>
                                    </div>
                                    <Check
                                        className={cn(
                                            'ml-auto',
                                            selectedPositionBatch?.batchId ===
                                                positionBatch.batchId
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
