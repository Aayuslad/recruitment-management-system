import { Badge } from './badge';
import { Tooltip, TooltipContent, TooltipTrigger } from './tooltip';

type Props = {
    id: string;
    name: string;
    type?: string | null;
    minExperienceYears?: number | null;
};

export function SkillPill({ id, name, type, minExperienceYears }: Props) {
    if (!type && !minExperienceYears) {
        return (
            <Badge
                key={id}
                variant="outline"
                className={`${type ? (type === 'Required' ? 'border-amber-950' : 'border-cyan-800') : 'border-slate-700'} text-sm font-normal pb-1 px-2.5 mr-1 mb-1`}
            >
                <span>{name}</span>
                {minExperienceYears !== 0 && (
                    <span className="text-xs -mb-1 pb-[1px] px-1.5 bg-accent rounded-2xl">
                        {minExperienceYears}
                    </span>
                )}
            </Badge>
        );
    }

    return (
        <Tooltip key={id}>
            <TooltipTrigger asChild>
                <Badge
                    variant="outline"
                    className={`${type ? (type === 'Required' ? 'border-amber-950' : 'border-cyan-800') : 'border-slate-700'} text-sm font-normal pb-1 px-2.5 mr-1 mb-1`}
                >
                    <span>{name}</span>
                    {minExperienceYears !== 0 && (
                        <span className="text-xs -mb-1 pb-[1px] px-1.5 bg-accent rounded-2xl">
                            {minExperienceYears}
                        </span>
                    )}
                </Badge>
            </TooltipTrigger>
            <TooltipContent>
                {type && (
                    <p className="space-x-1.5 mb-1">
                        <span>{type} skill</span>
                    </p>
                )}
                {minExperienceYears !== 0 && (
                    <p className="space-x-1.5">
                        <span className="font-semibold">
                            Minimum Experience:
                        </span>
                        <span>{minExperienceYears} years</span>
                    </p>
                )}
            </TooltipContent>
        </Tooltip>
    );
}
