// 'use client';

// import { CalendarIcon } from 'lucide-react';
// import { format } from 'date-fns';
// import { Control, useController } from 'react-hook-form';

// import { cn } from '@/lib/utils';
// import { Button } from '@/components/ui/button';
// import { Calendar } from '@/components/ui/calendar';
// import {
//     Popover,
//     PopoverContent,
//     PopoverTrigger,
// } from '@/components/ui/popover';
// import { ScrollArea, ScrollBar } from '@/components/ui/scroll-area';
// import {
//     FormControl,
//     FormItem,
//     FormLabel,
//     FormMessage,
// } from '@/components/ui/form';

// type Props<T> = {
//     control: Control<T>;
//     name: keyof T & string;
//     label?: string;
// };

// export function IsoDateTimePicker<T>({
//     control,
//     name,
//     label = 'Date & Time',
// }: Props<T>) {
//     const { field, fieldState } = useController({
//         name,
//         control,
//     });

//     const dateValue = field.value ? new Date(field.value) : undefined;

//     function updateDate(date: Date) {
//         const base = dateValue ?? new Date();
//         const updated = new Date(base);

//         updated.setFullYear(
//             date.getFullYear(),
//             date.getMonth(),
//             date.getDate()
//         );

//         field.onChange(updated.toISOString());
//     }

//     function updateTime(type: 'hour' | 'minute', value: string) {
//         const base = dateValue ?? new Date();
//         const updated = new Date(base);

//         if (type === 'hour') {
//             updated.setHours(parseInt(value, 10));
//         } else {
//             updated.setMinutes(parseInt(value, 10));
//         }

//         field.onChange(updated.toISOString());
//     }

//     return (
//         <FormItem className="flex flex-col">
//             {label && <FormLabel>{label}</FormLabel>}

//             <Popover>
//                 <PopoverTrigger asChild>
//                     <FormControl>
//                         <Button
//                             variant="outline"
//                             className={cn(
//                                 'w-full pl-3 text-left font-normal',
//                                 !field.value && 'text-muted-foreground'
//                             )}
//                         >
//                             {dateValue
//                                 ? format(dateValue, 'MM/dd/yyyy HH:mm')
//                                 : 'MM/DD/YYYY HH:mm'}
//                             <CalendarIcon className="ml-auto h-4 w-4 opacity-50" />
//                         </Button>
//                     </FormControl>
//                 </PopoverTrigger>

//                 <PopoverContent className="w-auto p-0">
//                     <div className="sm:flex">
//                         <Calendar
//                             mode="single"
//                             selected={dateValue}
//                             onSelect={(date) => date && updateDate(date)}
//                             initialFocus
//                         />

//                         <div className="flex flex-col sm:flex-row sm:h-[300px] divide-y sm:divide-y-0 sm:divide-x">
//                             <ScrollArea className="w-64 sm:w-auto">
//                                 <div className="flex sm:flex-col p-2">
//                                     {Array.from({ length: 24 }, (_, i) => i)
//                                         .reverse()
//                                         .map((hour) => (
//                                             <Button
//                                                 key={hour}
//                                                 size="icon"
//                                                 variant={
//                                                     dateValue &&
//                                                     dateValue.getHours() ===
//                                                         hour
//                                                         ? 'default'
//                                                         : 'ghost'
//                                                 }
//                                                 className="sm:w-full aspect-square"
//                                                 onClick={() =>
//                                                     updateTime(
//                                                         'hour',
//                                                         hour.toString()
//                                                     )
//                                                 }
//                                             >
//                                                 {hour}
//                                             </Button>
//                                         ))}
//                                 </div>
//                                 <ScrollBar
//                                     orientation="horizontal"
//                                     className="sm:hidden"
//                                 />
//                             </ScrollArea>

//                             <ScrollArea className="w-64 sm:w-auto">
//                                 <div className="flex sm:flex-col p-2">
//                                     {Array.from(
//                                         { length: 12 },
//                                         (_, i) => i * 5
//                                     ).map((minute) => (
//                                         <Button
//                                             key={minute}
//                                             size="icon"
//                                             variant={
//                                                 dateValue &&
//                                                 dateValue.getMinutes() ===
//                                                     minute
//                                                     ? 'default'
//                                                     : 'ghost'
//                                             }
//                                             className="sm:w-full aspect-square"
//                                             onClick={() =>
//                                                 updateTime(
//                                                     'minute',
//                                                     minute.toString()
//                                                 )
//                                             }
//                                         >
//                                             {minute.toString().padStart(2, '0')}
//                                         </Button>
//                                     ))}
//                                 </div>
//                                 <ScrollBar
//                                     orientation="horizontal"
//                                     className="sm:hidden"
//                                 />
//                             </ScrollArea>
//                         </div>
//                     </div>
//                 </PopoverContent>
//             </Popover>

//             {fieldState.error && <FormMessage />}
//         </FormItem>
//     );
// }
