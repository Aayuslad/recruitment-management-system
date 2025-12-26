import { Button } from '@/components/ui/button';
import {
    Sheet,
    SheetClose,
    SheetContent,
    SheetDescription,
    SheetFooter,
    SheetHeader,
    SheetTitle,
    SheetTrigger,
} from '@/components/ui/sheet';
import type { CreateCandidateCommandCorrected } from '@/types/candidate-types';
import { useState } from 'react';
import { useFieldArray, useForm } from 'react-hook-form';
import z from 'zod';
import { zodResolver } from '@hookform/resolvers/zod';
import { toast } from 'sonner';
import { useCreateCandidate } from '@/api/candidate-api';
import { Label } from '../ui/label';
import { Input } from '../ui/input';
import { CandidateSkillsSelector } from './internal/candidate-skills-selector';
import {
    Select,
    SelectContent,
    SelectGroup,
    SelectItem,
    SelectLabel,
    SelectTrigger,
    SelectValue,
} from '../ui/select';
import type { Gender } from '@/types/enums';
import { useAccessChecker } from '@/hooks/use-has-access';

const createCandidateFormSchema = z.object({
    email: z.email('Invalid email address'),
    firstName: z.string().min(1, 'First name is required'),
    middleName: z.string().optional().nullable(),
    lastName: z.string().min(1, 'Last name is required'),
    gender: z.enum(['Male', 'Female', 'Other', 'PreferNotToSay']).optional(),
    contactNumber: z.string().min(1, 'Contact number is required'),
    dob: z.string(),
    collegeName: z.string().min(1, 'College name is required'),
    resumeUrl: z.string().min(1, 'Resume URL is required'),
    skills: z.array(
        z.object({
            skillId: z.string(),
        })
    ),
}) satisfies z.ZodType<CreateCandidateCommandCorrected>;

type Props = {
    visibleTo: string[];
};

export function CreateCandidateSheet({ visibleTo }: Props) {
    const [open, setOpen] = useState(false);
    const createCandidateMutation = useCreateCandidate();
    const canAccess = useAccessChecker();

    const form = useForm<z.infer<typeof createCandidateFormSchema>>({
        resolver: zodResolver(createCandidateFormSchema),
    });

    const candidateSkillsFieldArray = useFieldArray({
        control: form.control,
        name: 'skills',
    });

    const onSubmit = async (data: CreateCandidateCommandCorrected) => {
        const contactNumber = data.contactNumber.trim();
        const dob = data.dob.trim();
        data.contactNumber = contactNumber.replace(/[\s\-()]/g, '');
        data.dob = new Date(dob).toISOString();
        createCandidateMutation.mutate(data, {
            onSuccess: () => {
                form.reset();
                setOpen(false);
            },
        });
    };

    const onInvalid = (errors: typeof form.formState.errors) => {
        const messages = Object.values(errors).map((err) => err.message);
        messages.reverse().forEach((msg) => toast.error(msg));
    };

    if (!canAccess(visibleTo)) return null;

    return (
        <Sheet open={open} onOpenChange={setOpen}>
            <SheetTrigger asChild>
                <Button variant="secondary" className="border">
                    + Create Candidate
                </Button>
            </SheetTrigger>
            <SheetContent className="w-[30vw]">
                <form
                    className="flex h-full flex-col gap-4"
                    onSubmit={form.handleSubmit(onSubmit, onInvalid)}
                >
                    <SheetHeader>
                        <SheetTitle>Create Candidate</SheetTitle>
                        <SheetDescription>
                            Add deatils for the candidate. Click create when
                            you&apos;re done.
                        </SheetDescription>
                    </SheetHeader>

                    <div
                        className="flex-1 grid auto-rows-min gap-6 px-4 overflow-y-auto py-1"
                        style={{
                            maxHeight:
                                'calc(100vh - var(--sheet-header-height))',
                        }}
                    >
                        <div className="grid gap-2">
                            <Label htmlFor="first-name">First Name</Label>
                            <Input
                                className="w-[300px]"
                                id="first-name"
                                {...form.register('firstName')}
                            />
                        </div>

                        <div className="grid gap-2">
                            <Label htmlFor="middle-name">Middle Name</Label>
                            <Input
                                className="w-[300px]"
                                id="middle-name"
                                {...form.register('middleName')}
                            />
                        </div>

                        <div className="grid gap-2">
                            <Label htmlFor="last-name">Last Name</Label>
                            <Input
                                className="w-[300px]"
                                id="last-name"
                                {...form.register('lastName')}
                            />
                        </div>

                        <div className="grid gap-3">
                            <Label htmlFor="gender">Gender</Label>
                            <Select
                                value={form.watch('gender')}
                                onValueChange={(value) =>
                                    form.setValue('gender', value as Gender)
                                }
                            >
                                <SelectTrigger className="w-[180px]">
                                    <SelectValue placeholder="Select a gender" />
                                </SelectTrigger>
                                <SelectContent>
                                    <SelectGroup>
                                        <SelectLabel>Gender</SelectLabel>
                                        <SelectItem value="Male">
                                            Male
                                        </SelectItem>
                                        <SelectItem value="Female">
                                            Female
                                        </SelectItem>
                                        <SelectItem value="Other">
                                            Other
                                        </SelectItem>
                                        <SelectItem value="PreferNotToSay">
                                            Prefer Not To Say
                                        </SelectItem>
                                    </SelectGroup>
                                </SelectContent>
                            </Select>
                        </div>

                        <div className="grid gap-2">
                            <Label htmlFor="email">Email</Label>
                            <Input
                                type="email"
                                className="w-[300px]"
                                id="email"
                                {...form.register('email')}
                            />
                        </div>

                        <div className="grid gap-2">
                            <Label htmlFor="contactNumber">
                                Contact Number
                            </Label>
                            <Input
                                id="contactNumber"
                                placeholder="+91 999999999"
                                className="w-[300px]"
                                {...form.register('contactNumber')}
                            />
                        </div>

                        <div className="grid gap-3">
                            <Label htmlFor="dob">Date of Birth</Label>
                            <Input
                                id="dob"
                                type="date"
                                className="w-[300px]"
                                {...form.register('dob')}
                            />
                        </div>

                        <div className="grid gap-2">
                            <Label htmlFor="last-name">College Name</Label>
                            <Input
                                className="w-[300px]"
                                id="college-name"
                                {...form.register('collegeName')}
                            />
                        </div>

                        <div className="grid gap-2">
                            <Label htmlFor="last-name">Resume URL</Label>
                            <Input
                                className="w-[300px]"
                                id="resume-url"
                                {...form.register('resumeUrl')}
                            />
                        </div>

                        <div>
                            <CandidateSkillsSelector
                                fields={candidateSkillsFieldArray.fields}
                                append={candidateSkillsFieldArray.append}
                                remove={candidateSkillsFieldArray.remove}
                            />
                        </div>
                    </div>

                    <SheetFooter className="flex flex-row w-full">
                        <Button type="submit" className="flex-1">
                            Create Candidate
                        </Button>
                        <SheetClose className="flex-1" asChild>
                            <Button variant="outline">Close</Button>
                        </SheetClose>
                    </SheetFooter>
                </form>
            </SheetContent>
        </Sheet>
    );
}
