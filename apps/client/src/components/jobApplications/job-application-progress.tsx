import type { JobApplicationStatus } from '@/types/enums';
import { Check } from 'lucide-react';

interface ApplicationProgressProps {
    status?: JobApplicationStatus;
}

const CORE_STATES = [
    'Applied',
    'Shortlisted',
    'Interviewed',
    'Offered',
    'Hired',
] as const;

export function ApplicationProgress({ status }: ApplicationProgressProps) {
    const currentIndex = CORE_STATES.indexOf(
        status as (typeof CORE_STATES)[number]
    );

    // Side states should not break the rail
    const isSideState = status === 'Rejected' || status === 'OnHold';

    return (
        <div className="space-y-3">
            {isSideState && (
                <span className="flex justify-end">
                    <span className="self-justify-end px-3 py-1 rounded-full text-sm font-semibold border">
                        {status}
                    </span>
                </span>
            )}

            {!isSideState && (
                <div className="relative flex items-center justify-between">
                    <div className="absolute left-0 right-0 top-1/2 h-0.5 -translate-y-1/2 bg-muted mx-4" />

                    {!isSideState && currentIndex >= 0 && (
                        <div
                            className="absolute left-0 top-1/2 h-0.5 -translate-y-1/2 bg-white transition-all mx-4"
                            style={{
                                width: `${(currentIndex / (CORE_STATES.length - 1)) * 98}%`,
                            }}
                        />
                    )}

                    {CORE_STATES.map((step, index) => {
                        const isCompleted = index < currentIndex;
                        const isCurrent = index === currentIndex;

                        return (
                            <div
                                key={step}
                                className="relative z-10 flex flex-col items-center gap-1"
                            >
                                <div
                                    className={`flex h-6 w-6 items-center justify-center rounded-full border text-xs font-semibold ${isCompleted ? 'bg-secondary-foreground text-accent' : isCurrent ? 'bg-white text-black' : 'border-muted bg-secondary text-muted-foreground'}`}
                                >
                                    {isCompleted ? (
                                        <Check className="w-4 h-4" />
                                    ) : (
                                        index + 1
                                    )}
                                </div>

                                {isCurrent && (
                                    <div className=" text-sm leading-none">
                                        ⌄
                                    </div>
                                )}

                                <span
                                    className={`text-sm ${isCompleted || isCurrent ? 'text-foreground' : 'text-muted-foreground'}`}
                                >
                                    {step}
                                </span>
                            </div>
                        );
                    })}
                </div>
            )}
        </div>
    );
}
