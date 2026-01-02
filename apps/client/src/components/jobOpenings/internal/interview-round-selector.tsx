'use client';

import * as React from 'react';

import { Button } from '@/components/ui/button';
import NumberInputWithEndButtons from '@/components/ui/number-input';
import { INTERVIEW_PARTICIPANT_ROLE } from '@/types/enums';
import type { CreateJobOpeningCommandCorrected } from '@/types/job-opening-types';
import { interviewParticipantRoleFormatConverter } from '@/util/interview-participant-role-format-converter';
import { durationFormatConverter } from '@/util/interview-round-duration-format-converter';
import { Save, SquarePen, X } from 'lucide-react';
import { toast } from 'sonner';
import { Label } from '../../ui/label';
import { InterviewRoundDurationSelector } from './interview-round-duration-selector';
import { InterviewRoundRolesRequirementSelector } from './interview-round-roles-requirement-selector';
import { InterviewRoundTypeSelector } from './interview-round-type-selector';

type Props = {
    fields: CreateJobOpeningCommandCorrected['interviewRounds'];
    append: (
        value: CreateJobOpeningCommandCorrected['interviewRounds'][0]
    ) => void;
    update: (
        index: number,
        value: CreateJobOpeningCommandCorrected['interviewRounds'][0]
    ) => void;
    remove: (index: number) => void;
};

export function InterviewRoundSelector({
    fields,
    append,
    remove,
    update,
}: Props) {
    return (
        <div>
            <div className="flex justify-between">
                <Label htmlFor="reviewers">Interview Rounds</Label>

                <Button
                    type="button"
                    variant="ghost"
                    role="combobox"
                    onClick={() => {
                        const roundNumber =
                            fields.reduce((acc, round) => {
                                return round.roundNumber > acc
                                    ? round.roundNumber
                                    : acc;
                            }, 0) + 1;

                        append({
                            type: 'Technical',
                            durationInMinutes: 60,
                            roundNumber: roundNumber,
                            requirements: [
                                {
                                    role: INTERVIEW_PARTICIPANT_ROLE.TECHNICAL_INTERVIEWER,
                                    requirementCount: 1,
                                },
                            ],
                        });
                    }}
                >
                    + Add Round
                </Button>
            </div>

            <div className="border rounded-2xl py-2 px-2 space-y-2">
                {fields?.map((field, index) => (
                    <RoundContent
                        key={index}
                        field={field}
                        index={index}
                        update={update}
                        remove={remove}
                    />
                ))}
                {fields?.length === 0 && (
                    <div className="text-muted-foreground text-center py-3">
                        No Interview Rounds added.
                    </div>
                )}
            </div>
        </div>
    );
}

type RoundContentProps = {
    field: CreateJobOpeningCommandCorrected['interviewRounds'][0];
    index: number;
    update: (
        index: number,
        value: CreateJobOpeningCommandCorrected['interviewRounds'][0]
    ) => void;
    remove: (index: number) => void;
};

const RoundContent = ({ field, index, update, remove }: RoundContentProps) => {
    const [edit, setEdit] = React.useState(true);

    return (
        <div key={index} className="flex items-center gap-2">
            {edit ? (
                <div className="flex-1 border rounded-2xl py-3 px-3 gap-6 pb-4 flex justify-around items-baseline">
                    <div className="space-y-3">
                        <div className="flex flex-col gap-2 w-[110px]">
                            <Label className="w-[130px]">Round Number</Label>
                            <NumberInputWithEndButtons
                                field={field.roundNumber}
                                name="roundNumber"
                                min={1}
                                small
                                increase={() =>
                                    update(index, {
                                        ...field,
                                        roundNumber: field.roundNumber + 1,
                                    })
                                }
                                decrease={() =>
                                    update(index, {
                                        ...field,
                                        roundNumber: field.roundNumber - 1,
                                    })
                                }
                            />
                        </div>
                        <div className="flex flex-col gap-2">
                            <Label className="w-[130px]">Duration</Label>
                            <InterviewRoundDurationSelector
                                selectedDuration={field.durationInMinutes}
                                setSelectedDuration={(duration) =>
                                    update(index, {
                                        ...field,
                                        durationInMinutes: duration,
                                    })
                                }
                            />
                        </div>
                        <div className="flex flex-col gap-2">
                            <Label className="w-[130px]">Interview Type</Label>
                            <InterviewRoundTypeSelector
                                selectedType={field.type}
                                setSelectedType={(type) =>
                                    update(index, { ...field, type: type })
                                }
                            />
                        </div>
                    </div>
                    <div className="flex-1 flex flex-col gap-2">
                        <span className="font-semibold">
                            Panel Requirements:
                        </span>
                        <div className="flex flex-col gap-2 pl-1">
                            {field.requirements.map((requirement) => (
                                <div
                                    key={requirement.role}
                                    className="flex items-center justify-between gap-2"
                                >
                                    <span className="font-normal">
                                        {interviewParticipantRoleFormatConverter(
                                            requirement.role
                                        )}
                                    </span>
                                    <div className="flex items-center w-[120px]">
                                        <NumberInputWithEndButtons
                                            field={requirement.requirementCount}
                                            name={requirement.role}
                                            min={1}
                                            small
                                            increase={() =>
                                                update(index, {
                                                    ...field,
                                                    requirements:
                                                        field.requirements.map(
                                                            (r) =>
                                                                r.role ===
                                                                requirement.role
                                                                    ? {
                                                                          ...r,
                                                                          requirementCount:
                                                                              r.requirementCount +
                                                                              1,
                                                                      }
                                                                    : r
                                                        ),
                                                })
                                            }
                                            decrease={() =>
                                                update(index, {
                                                    ...field,
                                                    requirements:
                                                        field.requirements.map(
                                                            (r) =>
                                                                r.role ===
                                                                requirement.role
                                                                    ? {
                                                                          ...r,
                                                                          requirementCount:
                                                                              r.requirementCount -
                                                                              1,
                                                                      }
                                                                    : r
                                                        ),
                                                })
                                            }
                                        />

                                        <button
                                            type="button"
                                            onClick={() => {
                                                update(index, {
                                                    ...field,
                                                    requirements:
                                                        field.requirements.filter(
                                                            (r) =>
                                                                r.role !==
                                                                requirement.role
                                                        ),
                                                });
                                            }}
                                            className="ml-2 rounded-full h-min hover:bg-muted hover:cursor-pointer"
                                        >
                                            <X className="h-4 w-4" />
                                        </button>
                                    </div>
                                </div>
                            ))}

                            <InterviewRoundRolesRequirementSelector
                                setSelectedRole={(role) => {
                                    const exist = field.requirements.find(
                                        (r) => r.role === role
                                    );

                                    if (exist) {
                                        toast('Role already exists');
                                        return;
                                    }

                                    update(index, {
                                        ...field,
                                        requirements: [
                                            ...field.requirements,
                                            {
                                                role: role,
                                                requirementCount: 1,
                                            },
                                        ],
                                    });
                                }}
                            />
                        </div>
                    </div>
                </div>
            ) : (
                <div className="flex-1 px-1">
                    <h4 className="font-semibold space-x-2.5">
                        <span>Round {field.roundNumber}</span>
                        <span>-</span>
                        <span>{field.type}</span>
                        <span>-</span>
                        <span>
                            {durationFormatConverter(field.durationInMinutes)}
                        </span>
                    </h4>
                    <div>
                        <span>Panel:</span>{' '}
                        {field.requirements.map(
                            (requirement, index) =>
                                `${requirement.requirementCount} ${interviewParticipantRoleFormatConverter(
                                    requirement.role,
                                    requirement.requirementCount > 1
                                )}${
                                    index === field.requirements.length - 1
                                        ? ''
                                        : ', '
                                }`
                        )}
                    </div>
                </div>
            )}

            <div className="flex flex-col h-full mb-auto">
                <Button
                    title={edit ? 'Save' : 'Edit'}
                    variant={'ghost'}
                    type="button"
                    className="w-7 h-7"
                    onClick={() => {
                        setEdit(!edit);
                    }}
                >
                    {edit ? <Save className="text-green-600" /> : <SquarePen />}
                </Button>
                <Button
                    title={'remove'}
                    variant={'ghost'}
                    type="button"
                    className="w-7 h-7"
                    onClick={() => {
                        remove(index);
                    }}
                >
                    <X />
                </Button>
            </div>
        </div>
    );
};
