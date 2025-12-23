import type { CreateJobOpeningCommandCorrected } from '@/types/job-opening-types';
import { MinusIcon, PlusIcon } from 'lucide-react';
import { Button, Group, Input } from 'react-aria-components';

type RoundNumberSelectorProps = {
    field: CreateJobOpeningCommandCorrected['interviewRounds'][0];
    increase: () => void;
    decrease: () => void;
};

export const RoundNumberSelector = ({
    field,
    increase,
    decrease,
}: RoundNumberSelectorProps) => {
    return (
        <Group className="dark:bg-input/30 border-input data-focus-within:border-ring data-focus-within:ring-ring/50 data-focus-within:has-aria-invalid:ring-destructive/20 dark:data-focus-within:has-aria-invalid:ring-destructive/40 data-focus-within:has-aria-invalid:border-destructive relative inline-flex h-7 w-[100px] min-w-0 items-center overflow-hidden rounded-md border bg-transparent text-base whitespace-nowrap shadow-xs transition-[color,box-shadow] outline-none data-disabled:pointer-events-none data-disabled:cursor-not-allowed data-disabled:opacity-50 data-focus-within:ring-[3px] md:text-sm">
            <Input
                value={field.roundNumber}
                disabled
                className="selection:bg-primary selection:text-primary-foreground w-full grow px-2 py-1 text-center tabular-nums outline-none"
            />
            <Button
                slot="decrement"
                className="border-input bg-background text-muted-foreground hover:bg-accent hover:text-foreground -me-px flex aspect-square h-[inherit] items-center justify-center border text-sm transition-[color,box-shadow] disabled:pointer-events-none disabled:cursor-not-allowed disabled:opacity-50"
                onClick={() => {
                    decrease();
                }}
            >
                <MinusIcon height={16} width={16} />
                <span className="sr-only">Decrement</span>
            </Button>
            <Button
                slot="increment"
                className="border-input bg-background text-muted-foreground hover:bg-accent hover:text-foreground -me-px flex aspect-square h-[inherit] items-center justify-center rounded-r-md border text-sm transition-[color,box-shadow] disabled:pointer-events-none disabled:cursor-not-allowed disabled:opacity-50"
                onClick={() => {
                    increase();
                }}
            >
                <PlusIcon height={16} width={16} />
                <span className="sr-only">Increment</span>
            </Button>
        </Group>
    );
};
